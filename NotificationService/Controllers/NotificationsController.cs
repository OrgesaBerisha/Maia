// Controllers/NotificationsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly NotificationDbContext _db;

    public NotificationsController(NotificationDbContext db) => _db = db;

    // GET api/notifications  — current user's notifications
    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var list = await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
        return Ok(list);
    }

    // PATCH api/notifications/{id}/read  — mark as read
    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var n = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (n is null) return NotFound();
        n.IsRead = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Admin only: GET all notifications
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .Take(200)
            .ToListAsync();
        return Ok(list);
    }

    // POST api/notifications/send — internal endpoint to send notifications
    [HttpPost("send")]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromBody] Notification n)
    {
        n.CreatedAt = DateTime.UtcNow;
        n.IsRead = false;
        _db.Notifications.Add(n);
        await _db.SaveChangesAsync();
        return Ok(n);
    }
}