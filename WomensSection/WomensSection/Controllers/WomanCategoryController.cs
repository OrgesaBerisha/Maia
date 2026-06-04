using Maia.Data;
using Maia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WomanCategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public WomanCategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.WomanCategories
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WomanCategoryDto dto)
        {
            var category = new WomanCategory { Name = dto.Name };
            _context.WomanCategories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(new { category.Id, category.Name });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WomanCategoryDto dto)
        {
            var existing = await _context.WomanCategories.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = dto.Name;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(new { existing.Id, existing.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.WomanCategories.FindAsync(id);
            if (existing == null) return NotFound();
            _context.WomanCategories.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class WomanCategoryDto
    {
        public required string Name { get; set; }
    }
}
