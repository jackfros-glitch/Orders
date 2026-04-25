using Orders.Entities.Models;

namespace Orders.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync(bool trackChanges);
        Task<Customer> GetCustomerAsync(Guid Id, bool trackChanges);

        void CreateCustomer(Customer customer);
        Task<IEnumerable<Customer>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCustomer(Customer customer);
    }
}
