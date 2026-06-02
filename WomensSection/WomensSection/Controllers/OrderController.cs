using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        // TODO: replace with real userId from JWT once auth is integrated by the team
        private int GetUserId() => 1;

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            await _service.CreateOrderAsync(GetUserId(), dto);
            return Ok("Order created");
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            return Ok(await _service.GetOrdersAsync(GetUserId()));
        }
    }
}
