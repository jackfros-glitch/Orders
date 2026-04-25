using Microsoft.EntityFrameworkCore;
using Orders.Entities.Models;
using Orders.Contracts;

namespace Orders.Repository
{
	public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
	{
		public CustomerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        	{

        	}

        public void CreateCustomer(Customer customer)=> Create(customer);
       

        public void DeleteCustomer(Customer customer)=> Delete(customer);


        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(bool trackChanges)=> await FindAll(trackChanges).OrderBy(p =>
        		p.Id).ToListAsync();

        public async Task<Customer> GetCustomerAsync(Guid Id, bool trackChanges) => await FindByCondition(c => c.Id.Equals(Id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Customer>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

    } 
}
