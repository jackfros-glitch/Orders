using Orders.Entities.Models;
using Orders.Shared.DataTransferObjects;

namespace Orders.Service.Contracts;

public interface IOrderService
{
    // Task<OrderDto> GetOrder(Guid orderId);
    Task<OrderDto> PlaceOrder(Guid userId, PlaceOrderDto order);

    Task<IEnumerable<OrderDto>> GetOrders();
    Task<OrderDto> CreateOrder(CreateOrderRequestDto orderRequest);

    Task<Order> GetOrder(Guid orderId);
}