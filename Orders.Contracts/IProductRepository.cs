using Orders.Entities.Models;

namespace Orders.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        Task<Product> GetProductAsync(Guid Id, bool trackChanges);
        void CreateProduct(Product product);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteProduct(Product product);
    }
}
