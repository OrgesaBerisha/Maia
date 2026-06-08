using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Data.DTO;
using OrderService.Models;
using OrderService.Services;
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
        private readonly IEmailService _email;
        private const string NotifUrl = "http://localhost:5151/api/notifications/send";

        public OrderController(OrderDbContext context, IHttpClientFactory http, IEmailService email)
        {
            _context = context;
            _http = http;
            _email = email;
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

            if (!string.IsNullOrEmpty(order.Email))
                _ = SendInvoiceEmail(order);

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

        private async Task SendInvoiceEmail(Order order)
        {
            try
            {
                var itemsHtml = string.Join("", order.OrderItems.Select(i => $@"
                    <tr>
                        <td style='padding:10px;border-bottom:1px solid #f0ece6;'>{i.ProductName}</td>
                        <td style='padding:10px;border-bottom:1px solid #f0ece6;text-align:center;'>{i.Quantity}</td>
                        <td style='padding:10px;border-bottom:1px solid #f0ece6;text-align:right;'>{i.Price:F2} EUR</td>
                        <td style='padding:10px;border-bottom:1px solid #f0ece6;text-align:right;'>{i.Price * i.Quantity:F2} EUR</td>
                    </tr>"));

                var html = $@"
<!DOCTYPE html>
<html>
<head><meta charset='utf-8'></head>
<body style='margin:0;padding:0;background:#f5f0eb;font-family:Helvetica Neue,Arial,sans-serif;'>
  <div style='max-width:600px;margin:40px auto;background:#fff;'>

    <div style='background:#1a1208;padding:32px 40px;'>
      <h1 style='color:#d4c5b3;font-family:Georgia,serif;font-size:28px;font-weight:400;margin:0;letter-spacing:0.1em;'>MAIA</h1>
      <p style='color:#9a8e82;font-size:12px;letter-spacing:0.2em;margin:4px 0 0;'>ORDER CONFIRMATION</p>
    </div>

    <div style='padding:32px 40px;'>
      <p style='color:#1a1208;font-size:14px;'>Dear <strong>{order.FullName}</strong>,</p>
      <p style='color:#6b5e52;font-size:13px;line-height:1.6;'>Thank you for your order. Here is your invoice summary.</p>

      <div style='background:#f5f0eb;padding:16px 20px;margin:24px 0;border-left:3px solid #1a1208;'>
        <p style='margin:0;font-size:12px;letter-spacing:0.15em;color:#6b5e52;'>ORDER #<strong style='color:#1a1208;'>{order.Id}</strong> &nbsp;·&nbsp; {order.CreatedAt:dd MMM yyyy HH:mm} &nbsp;·&nbsp; Status: <strong>{order.Status}</strong></p>
      </div>

      <h3 style='font-size:11px;letter-spacing:0.2em;color:#9a8e82;font-weight:400;border-bottom:1px solid #e8e0d8;padding-bottom:8px;'>ORDER ITEMS</h3>
      <table style='width:100%;border-collapse:collapse;font-size:13px;'>
        <thead>
          <tr style='background:#f5f0eb;'>
            <th style='padding:10px;text-align:left;font-size:10px;letter-spacing:0.15em;color:#9a8e82;font-weight:400;'>PRODUCT</th>
            <th style='padding:10px;text-align:center;font-size:10px;letter-spacing:0.15em;color:#9a8e82;font-weight:400;'>QTY</th>
            <th style='padding:10px;text-align:right;font-size:10px;letter-spacing:0.15em;color:#9a8e82;font-weight:400;'>PRICE</th>
            <th style='padding:10px;text-align:right;font-size:10px;letter-spacing:0.15em;color:#9a8e82;font-weight:400;'>SUBTOTAL</th>
          </tr>
        </thead>
        <tbody>{itemsHtml}</tbody>
        <tfoot>
          <tr>
            <td colspan='3' style='padding:14px 10px;text-align:right;font-size:13px;letter-spacing:0.1em;color:#1a1208;font-weight:600;'>TOTAL</td>
            <td style='padding:14px 10px;text-align:right;font-size:15px;font-weight:700;color:#1a1208;'>{order.TotalPrice:F2} EUR</td>
          </tr>
        </tfoot>
      </table>

      <div style='display:flex;gap:24px;margin-top:32px;'>
        <div style='flex:1;'>
          <h3 style='font-size:11px;letter-spacing:0.2em;color:#9a8e82;font-weight:400;border-bottom:1px solid #e8e0d8;padding-bottom:8px;'>DELIVERY ADDRESS</h3>
          <p style='font-size:13px;color:#1a1208;line-height:1.7;margin:0;'>
            {order.FullName}<br/>
            {order.Address}<br/>
            {order.PostalCode} {order.City}<br/>
            {order.Country}<br/>
            {order.Phone}
          </p>
        </div>
        <div style='flex:1;'>
          <h3 style='font-size:11px;letter-spacing:0.2em;color:#9a8e82;font-weight:400;border-bottom:1px solid #e8e0d8;padding-bottom:8px;'>PAYMENT</h3>
          <p style='font-size:13px;color:#1a1208;margin:0;'>{order.PaymentMethod ?? "Card"}</p>
        </div>
      </div>
    </div>

    <div style='background:#1a1208;padding:20px 40px;text-align:center;'>
      <p style='color:#9a8e82;font-size:11px;letter-spacing:0.15em;margin:0;'>MAIA &nbsp;·&nbsp; Thank you for shopping with us</p>
    </div>
  </div>
</body>
</html>";

                await _email.SendAsync(order.Email!, $"Order #{order.Id} Confirmed — MAIA", html);
            }
            catch { }
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
            catch { }
        }
    }
}
