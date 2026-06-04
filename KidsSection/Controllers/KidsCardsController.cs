using KidsSection.Data.DTO;
using KidsSection.Data.Interface;
using KidsSection.Models;
using Microsoft.AspNetCore.Mvc;

namespace KidsSection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidsCardsController : ControllerBase
    {
        private readonly IKidsCards _service;

        public KidsCardsController(IKidsCards service)
        {
            _service = service;
        }

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
        public async Task<IActionResult> Create([FromForm] CreateKidsCardsDto dto)
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
        public async Task<IActionResult> Search(string name)
        {
            var result = await _service.SearchAsync(name);
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
            return Ok(result);
        }
    }
}
