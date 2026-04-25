using Orders.Shared.DataTransferObjects;

namespace Orders.Service.Contracts;

public interface IOrderService
{
    // Task<OrderDto> GetOrder(Guid orderId);
    Task<OrderDto> PlaceOrder(Guid userId, PlaceOrderDto order);

    Task<List<OrderDto>> GetOrders();
}