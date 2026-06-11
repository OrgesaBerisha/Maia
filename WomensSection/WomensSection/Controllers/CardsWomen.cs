using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsWomenController : ControllerBase
    {
        private readonly ICardsWomenService _service;
        private readonly DataContext _db;
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public CardsWomenController(ICardsWomenService service, DataContext db, IHttpClientFactory http, IConfiguration config)
        {
            _service = service;
            _db = db;
            _http = http;
            _config = config;
        }

        private string NotifUrl => (_config["Services:Notifications"] ?? "http://localhost:5151") + "/api/notifications/send";

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            return Ok(await _service.GetByCategoryAsync(category));
        }

        [HttpGet("women")]
        public async Task<IActionResult> GetWomen()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("browse")]
        public async Task<IActionResult> Browse(
          [FromQuery] string? search,
          [FromQuery] int? categoryId,
          [FromQuery] decimal? minPrice,
          [FromQuery] decimal? maxPrice,
          [FromQuery] string? sortBy,
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 10)
        {
            var result = await _service.BrowseAsync(search, categoryId, minPrice, maxPrice, sortBy, page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCardsWomenDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCardsWomenDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPatch("{id}/sale")]
        public async Task<IActionResult> SetDiscount(int id, [FromBody] SetDiscountDto dto)
        {
            var result = await _service.SetDiscountAsync(id, dto.DiscountPercent);
            if (result == null) return NotFound();

            if (dto.DiscountPercent > 0)
            {
                var userIds = await _db.WishlistItems
                    .Where(wi => wi.ProductId == id)
                    .Select(wi => wi.Wishlist.UserId)
                    .Distinct()
                    .ToListAsync();

                foreach (var userId in userIds)
                    _ = SendNotification(userId.ToString(), "Item on Sale!",
                        $"{result.Title} is now {dto.DiscountPercent}% off — check it out!");
            }

            return Ok(result);
        }

        private async Task SendNotification(string userId, string title, string message)
        {
            try
            {
                var client = _http.CreateClient();
                var payload = JsonSerializer.Serialize(new { userId, title, message, type = "sale" });
                await client.PostAsync(NotifUrl, new StringContent(payload, Encoding.UTF8, "application/json"));
            }
            catch { }
        }
    }
}
