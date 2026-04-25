using Microsoft.EntityFrameworkCore.Storage;
namespace Orders.Contracts
{
    public interface IRepositoryManager
    {
    	IProductRepository Product { get ; }
    	IOrderRepository Order {get;}
	    IOrderItemRepository OrderItem { get;}
        ICustomerRepository Customer { get; }
        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
