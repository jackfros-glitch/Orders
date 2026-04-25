using Microsoft.AspNetCore.Mvc;
using Orders.Service.Contracts;
using Orders.Shared.DataTransferObjects;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Orders.Entities.Models;

namespace Orders.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {

        private readonly IServiceManager _service;

        public ProductsController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of all Products
        /// </summary>
        /// <returns>The Products List</returns>
        /// <response code="200">Returns the list of products</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _service.ProductService.GetProducts();
            return Ok(products);
        }

        /// <summary>
        /// Gets a single product by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <returns>A single product</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id:guid}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _service.ProductService.GetProduct(id);   
            return Ok(product);
        }

    }
}

