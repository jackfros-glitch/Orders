using Orders.Entities.Models;

namespace Orders.Contracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges);
        Task<Order> GetOrderAsync(Guid id, bool trackChanges);
        void CreateOrder(Order order);
        Task<IEnumerable<Order>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteOrder(Order order);
    }
}
