using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Data.Repository.Interface;
using Maia.Models;

namespace Maia.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateOrderAsync(int userId, CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId    = userId,
                CreatedAt = DateTime.UtcNow
            };

            decimal total = 0;
            foreach (var item in dto.Items)
            {
                var product = await _repo.GetProductAsync(item.ProductId);
                if (product == null) continue;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity  = item.Quantity,
                    Price     = product.Price
                });
                total += product.Price * item.Quantity;
            }

            order.TotalPrice = total;
            await _repo.AddOrderAsync(order);
        }

        public async Task<object> GetOrdersAsync(int userId)
        {
            var orders = await _repo.GetByUserIdAsync(userId);
            return MapOrders(orders);
        }

        public async Task<object> GetAllOrdersAsync()
        {
            var orders = await _repo.GetAllAsync();
            return MapOrders(orders);
        }

        private static object MapOrders(IEnumerable<Order> orders) =>
            orders.Select(o => new
            {
                o.Id,
                o.UserId,
                o.TotalPrice,
                o.CreatedAt,
                Items = o.OrderItems.Select(i => new
                {
                    i.ProductId,
                    ProductName  = i.Product?.Title,
                    ProductImage = i.Product?.ImageUrl,
                    i.Quantity,
                    i.Price,
                    Subtotal     = i.Price * i.Quantity
                })
            });
    }
}
