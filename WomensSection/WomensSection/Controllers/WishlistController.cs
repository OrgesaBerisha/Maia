using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;

        public WishlistController(IWishlistService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToWishlistDto dto)
        {
            await _service.AddAsync(dto);
            return Ok("Added to wishlist");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Removed");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAsync());
        }
    }
}