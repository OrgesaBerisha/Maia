using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;

namespace Maia.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                CustomerName = dto.CustomerName,
                CreatedAt = DateTime.UtcNow
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = await _context.CardsWoman.FindAsync(item.ProductId);

                if (product == null)
                    continue;

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                total += product.Price * item.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalPrice = total;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}