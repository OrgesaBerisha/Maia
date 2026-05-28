using Microsoft.AspNetCore.Mvc;
using MenSection.Data;
using MenSection.Data.DTO;
using MenSection.Models;


namespace MenSection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenCategoryController : ControllerBase
    {

            private readonly DataContext _context;

            public MenCategoryController(DataContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var categories = _context.MenCategories
                    .Select(c => new MenCategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToList();

                return Ok(categories);
            }

            [HttpPost]
            public async Task<IActionResult> Create(MenCategoryDto dto)
            {
                var category = new MenCategory
                {
                    Name = dto.Name
                };

                _context.MenCategories.Add(category);
                await _context.SaveChangesAsync();

                return Ok(category);
            }
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, UpdateMenCategoryDto dto)
            {
                var existing = await _context.MenCategories.FindAsync(id);

                if (existing == null)
                    return NotFound();

                existing.Name = dto.Name;

                await _context.SaveChangesAsync();

                return Ok(existing);
            }
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var category = await _context.MenCategories.FindAsync(id);

                if (category == null)
                    return NotFound();

                _context.MenCategories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

