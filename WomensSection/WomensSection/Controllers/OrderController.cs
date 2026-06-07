using System.Security.Claims;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _service.GetAllOrdersAsync());
        }
    }
}
