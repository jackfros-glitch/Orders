using Microsoft.AspNetCore.Mvc;
using Orders.Service.Contracts;
using Orders.Shared.DataTransferObjects;
using Marvin.Cache.Headers;
using Orders.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Orders.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class OrdersController : ControllerBase
    {

        private readonly IServiceManager _service;

        public OrdersController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all orders in the system
        /// </summary>
        /// <returns>A list of orders</returns>
        /// <response code="200">Returns the list of orders</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var ordersDto = await _service.OrderService.GetOrders();
            return Ok(ordersDto);
        }

        
        [HttpGet("{id:guid}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
           throw new NotImplementedException();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto orderRequestDto)
        {
            var orderDto = await _service.OrderService.CreateOrder(orderRequestDto);
            return Ok(orderDto);
        }

        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRepayment(Guid id)
        {
            throw new NotImplementedException();
        }

        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

