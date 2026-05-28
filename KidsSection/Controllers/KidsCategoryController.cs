using KidsSection.Data;
using KidsSection.Data.DTO;
using KidsSection.Models;
using Microsoft.AspNetCore.Mvc;

namespace KidsSection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidsCategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public KidsCategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.KidsCategories
                .Select(c => new KidsCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KidsCategoryDto dto)
        {
            var category = new KidsCategory
            {
                Name = dto.Name
            };

            _context.KidsCategories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateKidsCategoryDto dto)
        {
            var existing = await _context.KidsCategories.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.KidsCategories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.KidsCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
