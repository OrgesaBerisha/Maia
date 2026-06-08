using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Data.DTO;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public CartController(OrderDbContext context)
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
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return Ok(new { items = Array.Empty<object>(), total = 0 });

            return Ok(new
            {
                CartId = cart.Id,
                Items = cart.CartItems.Select(i => new
                {
                    i.Id,
                    i.ProductId,
                    i.ProductSource,
                    i.ProductName,
                    i.ImageUrl,
                    i.Price,
                    i.Quantity,
                    i.Size,
                    Subtotal = i.Price * i.Quantity
                }),
                Total = cart.CartItems.Sum(i => i.Price * i.Quantity)
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

            var existing = await _context.CartItems.FirstOrDefaultAsync(
                i => i.CartId == cart.Id && i.ProductId == dto.ProductId && i.ProductSource == dto.ProductSource);

            if (existing != null)
                existing.Quantity += dto.Quantity;
            else
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    ProductSource = dto.ProductSource,
                    ProductName = dto.ProductName,
                    ImageUrl = dto.ImageUrl,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    Size = dto.Size
                });

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

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
            return Ok("Cart cleared");
        }
    }
}
