using KidsSection.Data;
using KidsSection.Models;
using Microsoft.AspNetCore.Mvc;

namespace KidsSection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidsProductTypeController : ControllerBase
    {
        private readonly DataContext _context;

        public KidsProductTypeController(DataContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.KidsProductTypes.ToList());
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(KidsProductType type)
        {
            _context.KidsProductTypes.Add(type);
            await _context.SaveChangesAsync();
            return Ok(type);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, KidsProductType type)
        {
            var existing = await _context.KidsProductTypes.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.Name = type.Name;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.KidsProductTypes.FindAsync(id);

            if (existing == null)
                return NotFound();

            _context.KidsProductTypes.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
