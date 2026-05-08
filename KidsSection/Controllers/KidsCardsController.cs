using Microsoft.AspNetCore.Mvc;
using KidsSection.Data.Interface;
using KidsSection.Data.DTO;

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
        public async Task<IActionResult> Create(CreateKidsCardsDto dto)
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

    }
}
