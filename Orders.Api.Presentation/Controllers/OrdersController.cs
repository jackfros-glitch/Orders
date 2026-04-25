using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrdersController(IServiceManager service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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

        /// <summary>
        /// Retrieves a specific order by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the order</param>
        /// <returns>The requested order</returns>
        /// <response code="200">Order retrieved successfully</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id:guid}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
           var order = await _service.OrderService.GetOrder(id);
           var orderDto = _mapper.Map<OrderDto>(order);
           return Ok(orderDto);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="orderRequestDto">The order creation request payload</param>
        /// <returns>The created order</returns>
        /// <response code="201">Order created successfully</response>
        /// <response code="400">Invalid request or insufficient stock</response>
        /// <response code="404">One or more products not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto orderRequestDto)
        {
            var orderDto = await _service.OrderService.CreateOrder(orderRequestDto);
            return CreatedAtAction(
                nameof(GetOrder),
                new { id = orderDto.Id },
                orderDto
            );
        }

    }
}

