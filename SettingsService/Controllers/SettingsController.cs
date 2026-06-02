using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly SettingsDbContext _db;

    public SettingsController(SettingsDbContext db) => _db = db;

    [HttpGet("global")]
    public async Task<IActionResult> GetGlobal()
    {
        var settings = await _db.Settings
            .Where(s => s.Scope == "global")
            .ToListAsync();
        return Ok(settings);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserSettings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var settings = await _db.Settings
            .Where(s => s.Scope == "user" && s.OwnerId == userId)
            .ToListAsync();
        return Ok(settings);
    }

    [HttpPost("global")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetGlobal([FromBody] SettingRequest request)
    {
        var existing = await _db.Settings
            .FirstOrDefaultAsync(s => s.Key == request.Key && s.Scope == "global");

        if (existing is null)
        {
            _db.Settings.Add(new Setting
            {
                Key   = request.Key,
                Value = request.Value,
                Scope = "global"
            });
        }
        else
        {
            existing.Value     = request.Value;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("user")]
    public async Task<IActionResult> SetUserSetting([FromBody] SettingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var existing = await _db.Settings
            .FirstOrDefaultAsync(s => s.Key == request.Key
                && s.Scope == "user"
                && s.OwnerId == userId);

        if (existing is null)
        {
            _db.Settings.Add(new Setting
            {
                Key     = request.Key,
                Value   = request.Value,
                Scope   = "user",
                OwnerId = userId
            });
        }
        else
        {
            existing.Value     = request.Value;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var setting = await _db.Settings.FindAsync(id);
        if (setting is null) return NotFound();
        _db.Settings.Remove(setting);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public class SettingRequest
{
    public string Key   { get; set; } = "";
    public string Value { get; set; } = "";
}
