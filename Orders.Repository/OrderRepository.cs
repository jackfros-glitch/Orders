using Microsoft.EntityFrameworkCore;
using Orders.Entities.Models;
using Orders.Contracts;

namespace Orders.Repository
{
	public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
		public OrderRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        	{

        	}

        public void CreateOrder(Order order)=> Create(order);
       

        public void DeleteOrder(Order order)=> Delete(order);


        public async Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges)=> await FindAll(trackChanges).OrderBy(p =>
        		p.Id).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product).ToListAsync();

        public async Task<Order> GetOrderAsync(Guid Id, bool trackChanges) => await FindByCondition(p => p.Id.Equals(Id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Order>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

    } 
}
