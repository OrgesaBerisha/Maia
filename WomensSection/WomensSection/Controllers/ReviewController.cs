using System.Security.Claims;
using Maia.Data;
using Maia.Data.DTO;
using Maia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maia.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly DataContext _ctx;
        public ReviewController(DataContext ctx) => _ctx = ctx;

        private int? GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim == null ? null : int.Parse(claim);
        }

        private string GetUserName() =>
            User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue(ClaimTypes.Email)
            ?? "User";

        // GET /api/reviews?productId=5&source=WOMAN
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int productId, [FromQuery] string source = "WOMAN")
        {
            var reviews = await _ctx.ProductReviews
                .Where(r => r.ProductId == productId && r.Source == source)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.UserId,
                    r.UserName,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt
                })
                .ToListAsync();

            var avg = reviews.Count > 0 ? reviews.Average(r => r.Rating) : 0.0;

            return Ok(new { average = Math.Round(avg, 1), count = reviews.Count, reviews });
        }

        // GET /api/reviews/summary?productId=5&source=WOMAN  (lightweight — just avg + count)
        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] int productId, [FromQuery] string source = "WOMAN")
        {
            var ratings = await _ctx.ProductReviews
                .Where(r => r.ProductId == productId && r.Source == source)
                .Select(r => r.Rating)
                .ToListAsync();

            var avg = ratings.Count > 0 ? Math.Round(ratings.Average(), 1) : 0.0;
            return Ok(new { average = avg, count = ratings.Count });
        }

        // POST /api/reviews
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddReviewDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var source = string.IsNullOrEmpty(dto.Source) ? "WOMAN" : dto.Source.ToUpper();

            var existing = await _ctx.ProductReviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == dto.ProductId && r.Source == source);

            if (existing != null)
            {
                existing.Rating  = dto.Rating;
                existing.Comment = dto.Comment;
                existing.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                _ctx.ProductReviews.Add(new ProductReview
                {
                    UserId    = userId.Value,
                    UserName  = GetUserName(),
                    ProductId = dto.ProductId,
                    Source    = source,
                    Rating    = dto.Rating,
                    Comment   = dto.Comment,
                });
            }

            await _ctx.SaveChangesAsync();
            return Ok("Review saved");
        }

        // DELETE /api/reviews/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var review = await _ctx.ProductReviews.FindAsync(id);
            if (review == null) return NotFound();

            var isAdmin = User.IsInRole("Admin");
            if (review.UserId != userId && !isAdmin) return Forbid();

            _ctx.ProductReviews.Remove(review);
            await _ctx.SaveChangesAsync();
            return Ok("Deleted");
        }
    }
}
