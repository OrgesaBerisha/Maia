using KidsSection.Data.DTO;
using KidsSection.Data.Interface;
using KidsSection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace KidsSection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidsCardsController : ControllerBase
    {
        private readonly IKidsCards _service;
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public KidsCardsController(IKidsCards service, IHttpClientFactory http, IConfiguration config)
        {
            _service = service;
            _http = http;
            _config = config;
        }

        private string WomensUrl => (_config["Services:Womens"] ?? "http://localhost:5182") + "/api/wishlist/notify-sale";

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            return Ok(await _service.GetByCategoryAsync(categoryId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateKidsCardsDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
        // DELETE: api/KidsViewAllCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
        // UPDATE (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateKidsCardsDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        //SEARCH
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            var result = await _service.SearchAsync(query);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string? name, int? categoryId, decimal? min, decimal? max)
        {
            return Ok(await _service.FilterAsync(name, categoryId, min, max));
        }

 

        [HttpGet("sort")]
        public async Task<IActionResult> Sort(SortOptions sortBy)
        {
            return Ok(await _service.SortAsync(sortBy));
        }

        [HttpPatch("{id}/sale")]
        public async Task<IActionResult> SetSale(int id, [FromBody] SetDiscountDto dto)
        {
            var result = await _service.SetDiscountAsync(id, dto.DiscountPercent);
            if (result == null) return NotFound();

            if (dto.DiscountPercent > 0)
                _ = NotifyWishlistUsers(id, result.Title ?? "Kids' item", dto.DiscountPercent.Value);

            return Ok(result);
        }

        private async Task NotifyWishlistUsers(int productId, string title, int discount)
        {
            try
            {
                var client = _http.CreateClient();
                var payload = JsonSerializer.Serialize(new
                {
                    productId,
                    source = "KIDS",
                    title = "Item on Sale!",
                    message = $"{title} is now {discount}% off — check it out!"
                });
                await client.PostAsync(WomensUrl, new StringContent(payload, Encoding.UTF8, "application/json"));
            }
            catch { }
        }
    }
}
