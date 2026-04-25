using Orders.Shared.DataTransferObjects;
using Orders.Entities.Models;

namespace Orders.Service.Contracts;

public interface IProductService
{
    Task<ProductDto> GetProduct(Guid productId);

    Task<IEnumerable<ProductDto>> GetProducts();
    Task<IEnumerable<Product>> GetProductsById(List<Guid> productIds, bool trackChanges = false);

    // bool ValidateProductsExist(List<Guid> requestedIds, IEnumerable<ProductDto> foundProducts);
}