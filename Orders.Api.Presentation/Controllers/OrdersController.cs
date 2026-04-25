using Microsoft.AspNetCore.Mvc;
using Orders.Service.Contracts;
using Orders.Shared.DataTransferObjects;
using Marvin.Cache.Headers;

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

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            throw new NotImplementedException();
        }

        
        [HttpGet("{id:guid}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            throw new NotImplementedException();
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

