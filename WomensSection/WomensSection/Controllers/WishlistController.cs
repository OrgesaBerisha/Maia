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

        // TODO: replace with real userId from JWT once auth is integrated by the team
        private int GetUserId() => 1;

        [HttpPost]
        public async Task<IActionResult> Add(AddToWishlistDto dto)
        {
            await _service.AddAsync(GetUserId(), dto);
            return Ok("Added to wishlist");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _service.RemoveAsync(GetUserId(), id);
            return Ok("Removed");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAsync(GetUserId()));
        }
    }
}
