using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsWomenController : ControllerBase
    {
        private readonly ICardsWomenService _service;

        public CardsWomenController(ICardsWomenService service)
        {
            _service = service;
        }

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

        // 👇 WOMEN PAGE
        [HttpGet("women")]
        public async Task<IActionResult> GetWomen()
        {
            return Ok(await _service.GetAllAsync());
        }


        // 🚀 FILTER + SORT + PAGINATION (FINAL VERSION)
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
            var result = await _service.BrowseAsync(
                search,
                categoryId,
                minPrice,
                maxPrice,
                sortBy,
                page,
                pageSize);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCardsWomenDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCardsWomenDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
    }
}