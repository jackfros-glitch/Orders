using AutoMapper;
using Orders.Service.Contracts;
using Orders.Contracts;
using Orders.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using Orders.Entities.Models;
using Orders.Entities.Exceptions;

namespace Orders.Service;

public class ProductService : IProductService
{   
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IConfiguration _config;

    private readonly IMapper _mapper;
    public ProductService(IRepositoryManager repoManager, ILoggerManager loggerManager, IConfiguration configuration, IMapper mapper)
    {
        _repositoryManager = repoManager;
        _loggerManager = loggerManager;
        _config = configuration;   
        _mapper = mapper;
    }
    public async Task<ProductDto> GetProduct(Guid productId)
    {
       var product = await _repositoryManager.Product.GetProductAsync(productId, false);
       if (product is null) throw new ProductNotFoundException(productId);
       var productDto = _mapper.Map<ProductDto>(product);
       return productDto;
    }

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var products = await _repositoryManager.Product.GetAllProductsAsync(false);
        var productsDTO = _mapper.Map<IEnumerable<ProductDto>>(products);
        return productsDTO;
    }

    public async Task<IEnumerable<Product>> GetProductsById(List<Guid> productIds, bool trackChanges = false)
    {
        var products = await _repositoryManager.Product.GetByIdsAsync(productIds, trackChanges);
        // var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return products;
    }

    // public bool ValidateProductsExist(
    //     List<Guid> requestedIds,
    //     IEnumerable<ProductDto> foundProducts)
    // {
    //     // Validates if all the requested Products Exists 

    //     var returnedIds = foundProducts.Select(p => p.Id).ToHashSet();
    //     var notFoundIds = requestedIds.Where(id => !returnedIds.Contains(id)).ToList();

    //     if (notFoundIds.Any())
    //         return false;
    //     return true;
    // }

    
}