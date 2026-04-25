using Orders.Service.Contracts;
using Orders.Shared.DataTransferObjects;

namespace Orders.Service;

public class OrderService : IOrderService
{
    public Task<List<OrderDto>> GetOrders()
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> PlaceOrder(Guid userId, PlaceOrderDto order)
    {
        throw new NotImplementedException();
    }
}