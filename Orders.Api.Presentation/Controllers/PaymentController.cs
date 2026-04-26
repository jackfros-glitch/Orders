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
    public class PaymentController : ControllerBase
    {

        private readonly IServiceManager _service;
        private readonly IMapper _mapper;

        public PaymentController(IServiceManager service, IMapper mapper)
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
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
           throw new NotImplementedException();
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
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPayment(Guid id)
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// Generate payment Reference
        /// </summary>
        /// <returns>The generated payment reference</returns>
        /// <response code="200">Reference Generated successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">order not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("/api/payments/reference/{orderId:Guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentReferenceDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateReference([FromRoute] Guid orderId)
        {
            var paymentRefence = await _service.PaymentService.GeneratePaymentReference(orderId);
            var referenceDto = _mapper.Map<PaymentReferenceDto>(paymentRefence);
            return Ok(referenceDto);
        }


        /// <summary>
        /// Make payment
        /// </summary>
        /// <returns>Payment Details</returns>
        /// <response code="200">Payment successfully Made</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">order not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("/api/payments/pay/{reference:Guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentReferenceDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MakePayment([FromRoute] Guid reference)
        {
            var payment = await _service.PaymentService.Pay(reference);
            var paymentDto = _mapper.Map<PaymentDto>(payment);
            return Ok(paymentDto);
        }

    }
}

