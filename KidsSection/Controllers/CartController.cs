using KidsSection.Data;
using KidsSection.Data.DTO;
using KidsSection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KidsSection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return Ok(new { items = Array.Empty<object>(), total = 0 });

            var items = cart.CartItems.Select(i => new
            {
                i.Id,
                i.ProductId,
                ProductName = i.Product?.Title,
                ProductImage = i.Product?.ImageUrl,
                Price = i.Product?.Price,
                i.Quantity,
                Subtotal = i.Product?.Price * i.Quantity
            });

            return Ok(new
            {
                CartId = cart.Id,
                Items = items,
                Total = cart.CartItems.Sum(i => (i.Product?.Price ?? 0) * i.Quantity)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToCartDto dto)
        {
            var userId = GetUserId();
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == dto.ProductId);

            if (existing != null)
                existing.Quantity += dto.Quantity;
            else
                _context.CartItems.Add(new CartItem { CartId = cart.Id, ProductId = dto.ProductId, Quantity = dto.Quantity });

            await _context.SaveChangesAsync();
            return Ok("Added to cart");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return NotFound();
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Removed");
        }
    }
}
