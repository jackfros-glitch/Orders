using Orders.Service.Contracts;
using Orders.Contracts;
using Orders.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Orders.Entities.Models;
using System.Runtime.InteropServices;
using Orders.Entities.Exceptions;
using Microsoft.VisualBasic;
using System.Text.Json;
using Orders.Shared.Enums;

namespace Orders.Service;

public class OrderService : IOrderService
{   
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IConfiguration _config;

    private readonly IMapper _mapper;
    private readonly IServiceManager _service;

    public OrderService(
        IRepositoryManager repoManager, 
        ILoggerManager loggerManager, 
        IConfiguration configuration, 
        IMapper mapper, 
        IServiceManager serviceManager
        )
    {
        _repositoryManager = repoManager;
        _loggerManager = loggerManager;
        _config = configuration;   
        _mapper = mapper;
        _service = serviceManager;
    }
    public async Task<IEnumerable<OrderDto>> GetOrders()
    {
        var orders = await _repositoryManager.Order.GetAllOrdersAsync(false);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }


    public async Task<Order> GetOrder(Guid orderId)
    {
        var order = await _repositoryManager.Order.GetOrderAsync(orderId, eagerLoad: true);
        if (order is null)
        {
            throw new ProductNotFoundException(orderId);
        }
        return order;
    } 
    public async Task PlaceOrder(Order order)
    {
        // trigger the placeOrder(
        // validate the stock
        // update the stock 
        // update the order status )
        await using var transaction = await _repositoryManager.BeginTransactionAsync();
        try
        {
            // var order = await _repositoryManager.Order.GetOrderAsync(orderId);
            // if (order is null) throw new OrderNotFoundException(orderId);
            var productIds = order.OrderItems.Select(oi=> oi.ProductId);
            var products = await _repositoryManager.Product.GetByIdsAsync(productIds, true);
            // validate stock
            foreach (var product in products)
            {
                // Find the matching line item from the request
                var orderItem = order.OrderItems
                    .FirstOrDefault(item => item.ProductId == product.Id);

                if (orderItem is null) continue;

                if (orderItem.Quantity > product.Quantity)
                    throw new ProductBadRequestException(
                        $"The quantity selected for '{product.Name}' ({orderItem.Quantity}) " +
                        $"exceeds available stock ({product.Quantity}).");
            }
            // var productLookup = products.ToDictionary(p => p.Id);
            await UpdateProductStock(order.OrderItems);
            order.Status = OrderStatus.Placed;
            await _repositoryManager.SaveAsync();
            await transaction.CommitAsync();
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync(); // anything failed — undo everything
            throw new Exception(e.Message);
        }
        
    }
    public async Task<OrderDto> CreateOrder(CreateOrderRequestDto orderRequest)
    {
        var productIds = orderRequest.Products.Select(p => p.ProductId).ToList();
        var products = await _service.ProductService.GetProductsById(productIds, true);

        ValidateProductsExist(productIds, products);
        ValidateProductQuantities(orderRequest.Products, products);
        // var productLookup = products.ToDictionary(p => p.Id);

        using var transaction = await _repositoryManager.BeginTransactionAsync();

        var customer = new Customer
        {
            Name = "Guest",
            EmailAddress = "guest@test.com",
            PasswordHash = "none"
        };

        _repositoryManager.Customer.CreateCustomer(customer);

        await _repositoryManager.SaveAsync(); 
        var order = new Order{ CustomerId = customer.Id };
        _repositoryManager.Order.CreateOrder(order);
        CreateOrderItems(order, orderRequest.Products, products);

        // UpdateProductStock(orderRequest.Products, productLookup);

        var total = orderRequest.Products.Sum(p =>
        {
            var product = products.First(x => x.Id == p.ProductId);
            return product.Price * p.Quantity;
        });
        order.TotalAmount = total;

        await _repositoryManager.SaveAsync();

        await transaction.CommitAsync();
        var orderDto = _mapper.Map<OrderDto>(order);
        return orderDto; // map to OrderDto as needed
    }


    private async Task UpdateProductStock(
        IEnumerable<OrderItem> requestedItems)
    {
        foreach (var item in requestedItems)
        {
            // var product = productLookup[item.ProductId];

            // if (product.Quantity < item.Quantity)
            //     throw new ProductBadRequestException(
            //         $"Insufficient stock for {product.Name}");

            // product.Quantity -= item.Quantity;
            var success = await _repositoryManager.Product
                .DecrementStockAsync(item.ProductId, item.Quantity);

            if (!success)
                throw new ProductBadRequestException(
                    $"'{item.ProductId}' is out of stock or unavailable.");

        }
    }
    private void ValidateProductsExist(
        List<Guid> requestedIds,
        IEnumerable<Product> foundProducts)
    {
        var returnedIds = foundProducts.Select(p => p.Id).ToHashSet();
        var notFoundIds = requestedIds.Where(id => !returnedIds.Contains(id)).ToList();

        if (notFoundIds.Any())
            throw new ProductsNotFoundException(notFoundIds);
    }

    private void ValidateProductQuantities(
        IEnumerable<ProductRequestDto> requestedItems,
        IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            // Find the matching line item from the request
            var requestedItem = requestedItems
                .FirstOrDefault(item => item.ProductId == product.Id);

            if (requestedItem is null) continue;

            if (requestedItem.Quantity > product.Quantity)
                throw new ProductBadRequestException(
                    $"The quantity selected for '{product.Name}' ({requestedItem.Quantity}) " +
                    $"exceeds available stock ({product.Quantity}).");
        }
    }

    private void CreateOrderItems(
        Order order,
        IEnumerable<ProductRequestDto> requestedItems,
        IEnumerable<Product> products)
    {
        // Build a lookup to avoid repeated linear scans
        var productLookup = products.ToDictionary(p => p.Id);

        foreach (var item in requestedItems)
        {
            if (!productLookup.TryGetValue(item.ProductId, out var product))
                continue; // already guarded by ValidateProductsExist

            // var product = _mapper.Map<Product>(productItem);
            var orderItem = new OrderItem
            {
                Order     = order,
                ProductId   = product.Id,
                Quantity  = item.Quantity,
                UnitPrice = product.Price
            };

            _repositoryManager.OrderItem.CreateOrderItem(orderItem);
        
        }
    }


   
}