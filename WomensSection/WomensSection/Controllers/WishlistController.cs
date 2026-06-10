using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;
        private readonly DataContext _db;
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public WishlistController(IWishlistService service, DataContext db, IHttpClientFactory http, IConfiguration config)
        {
            _service = service;
            _db = db;
            _http = http;
            _config = config;
        }

        private string NotifUrl => (_config["Services:Notifications"] ?? "http://localhost:5151") + "/api/notifications/send";

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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

        [HttpPost("notify-sale")]
        [AllowAnonymous]
        public async Task<IActionResult> NotifySale([FromBody] NotifySaleDto dto)
        {
            var userIds = await _db.WishlistItems
                .Where(wi => wi.ProductId == dto.ProductId && wi.Source == dto.Source)
                .Select(wi => wi.Wishlist.UserId)
                .Distinct()
                .ToListAsync();

            foreach (var userId in userIds)
                _ = SendNotification(userId.ToString(), dto.Title, dto.Message);

            return Ok(new { notified = userIds.Count });
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

    public class NotifySaleDto
    {
        public int ProductId { get; set; }
        public string Source { get; set; } = "WOMAN";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
