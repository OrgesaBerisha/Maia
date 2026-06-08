using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Data.DTO;
using OrderService.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IHttpClientFactory _http;
        private const string NotifUrl = "http://localhost:5151/api/notifications/send";

        public OrderController(OrderDbContext context, IHttpClientFactory http)
        {
            _context = context;
            _http = http;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var userId = GetUserId();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            List<OrderItem> orderItems;

            if (dto.Items != null && dto.Items.Any())
            {
                orderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId     = i.ProductId,
                    ProductSource = i.ProductSource,
                    ProductName   = i.ProductName,
                    ImageUrl      = i.ImageUrl,
                    Price         = i.Price,
                    Quantity      = i.Quantity
                }).ToList();
            }
            else if (cart != null && cart.CartItems.Any())
            {
                orderItems = cart.CartItems.Select(i => new OrderItem
                {
                    ProductId     = i.ProductId,
                    ProductSource = i.ProductSource,
                    ProductName   = i.ProductName,
                    ImageUrl      = i.ImageUrl,
                    Price         = i.Price,
                    Quantity      = i.Quantity
                }).ToList();
            }
            else
            {
                return BadRequest("Cart is empty");
            }

            var order = new Order
            {
                UserId        = userId,
                Status        = "Pending",
                FullName      = dto.FullName,
                Email         = dto.Email,
                Phone         = dto.Phone,
                Address       = dto.Address,
                City          = dto.City,
                PostalCode    = dto.PostalCode,
                Country       = dto.Country,
                PaymentMethod = dto.PaymentMethod,
                OrderItems    = orderItems
            };

            order.TotalPrice = order.OrderItems.Sum(i => i.Price * i.Quantity);

            _context.Orders.Add(order);
            if (cart != null) _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            _ = SendNotification(userId.ToString(),
                "Order Placed",
                $"Your order #{order.Id} has been placed! Total: {order.TotalPrice:F2} EUR");

            return Ok(new { order.Id, order.TotalPrice, order.Status, order.CreatedAt });
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            _ = SendNotification(order.UserId.ToString(),
                "Order Update",
                $"Order #{order.Id} status: {dto.Status}");

            return Ok(new { order.Id, order.Status });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.TotalPrice,
                o.Status,
                o.CreatedAt,
                Items = o.OrderItems.Select(i => new
                {
                    i.ProductId,
                    i.ProductSource,
                    i.ProductName,
                    i.ImageUrl,
                    i.Price,
                    i.Quantity
                })
            }));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.UserId,
                o.FullName,
                o.Email,
                o.Phone,
                o.Address,
                o.City,
                o.PostalCode,
                o.Country,
                o.PaymentMethod,
                o.TotalPrice,
                o.Status,
                o.CreatedAt,
                Items = o.OrderItems.Select(i => new
                {
                    i.ProductId,
                    i.ProductName,
                    i.ImageUrl,
                    i.Price,
                    i.Quantity,
                    i.ProductSource
                })
            }));
        }

        private async Task SendNotification(string userId, string title, string message)
        {
            try
            {
                var client = _http.CreateClient();
                var payload = JsonSerializer.Serialize(new
                {
                    userId,
                    title,
                    message,
                    type = "inapp"
                });
                await client.PostAsync(NotifUrl,
                    new StringContent(payload, Encoding.UTF8, "application/json"));
            }
            catch { /* notification failure should not break the order flow */ }
        }
    }
}
