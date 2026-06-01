// Controllers/FileUploadController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/files")]
[Authorize]
public class FileUploadController : ControllerBase
{
    private readonly FileUploadDbContext _db;
    private readonly IStorageService _storage;

    public FileUploadController(FileUploadDbContext db, IStorageService storage)
    {
        _db = db;
        _storage = storage;
    }

    // POST api/files/upload
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
        IFormFile file, [FromQuery] string category)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Only JPEG, PNG and WEBP images are allowed.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File size must be under 5MB.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var url = await _storage.UploadAsync(file, category);

        var record = new FileRecord
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Url = url,
            UploadedBy = userId,
            Category = category,
            SizeInBytes = file.Length
        };

        _db.FileRecords.Add(record);
        await _db.SaveChangesAsync();

        return Ok(new { record.Id, record.Url, record.FileName });
    }

    // GET api/files?category=men
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? category)
    {
        var query = _db.FileRecords.AsQueryable();
        if (!string.IsNullOrEmpty(category))
            query = query.Where(f => f.Category == category);

        var files = await query
            .OrderByDescending(f => f.UploadedAt)
            .ToListAsync();

        return Ok(files);
    }

    // DELETE api/files/{id}
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var file = await _db.FileRecords.FindAsync(id);
        if (file is null) return NotFound();

        await _storage.DeleteAsync(file.FileName);
        _db.FileRecords.Remove(file);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}