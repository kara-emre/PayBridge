using Microsoft.AspNetCore.Mvc;
using PayPridge.Application.DTOs;
using PayPridge.Application.Interfaces;

namespace PayBridge.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderPaymentService _orderService;

        public OrderController(IOrderPaymentService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            return Ok(await _orderService.CreateOrderAsync(request));
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteOrder(Guid id)
        {
            return Ok(await _orderService.CompleteOrderAsync(id));
        }
    }
}
