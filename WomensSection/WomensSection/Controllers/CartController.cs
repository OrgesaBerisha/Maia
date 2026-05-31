using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToCartDto dto)
        {
            await _service.AddToCartAsync(dto);
            return Ok("Added to cart");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _service.RemoveFromCartAsync(id);
            return Ok("Removed");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetCartAsync());
        }
    }
}