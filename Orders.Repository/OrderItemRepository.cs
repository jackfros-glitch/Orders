using Microsoft.EntityFrameworkCore;
using Orders.Entities.Models;
using Orders.Contracts;

namespace Orders.Repository
{
	public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
	{
		public OrderItemRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        	{

        	}

        public void CreateOrderItem(OrderItem orderItem)=> Create(orderItem);

        public void DeleteOrderItem(OrderItem orderItem)=> Delete(orderItem);

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync(bool trackChanges)=> await FindAll(trackChanges).OrderBy(oi =>
        		oi.Id).ToListAsync();

        public async Task<OrderItem> GetOrderItemAsync(Guid Id, bool trackChanges) => await FindByCondition(oi => oi.Id.Equals(Id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<OrderItem>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

    } 
}
