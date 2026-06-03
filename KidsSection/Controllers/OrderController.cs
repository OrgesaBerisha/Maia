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
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var userId = GetUserId();
            var order = new Order { UserId = userId };

            foreach (var item in dto.Items)
            {
                var product = await _context.KidsCards.FindAsync(item.ProductId);
                if (product == null) continue;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
                order.TotalPrice += product.Price * item.Quantity;
            }

            _context.Orders.Add(order);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
                _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();
            return Ok("Order created");
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.TotalPrice,
                o.CreatedAt,
                Items = o.OrderItems.Select(i => new
                {
                    i.ProductId,
                    ProductName = i.Product?.Title,
                    i.Quantity,
                    i.Price
                })
            }));
        }
    }
}
