using Maia.Data.Repository.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CardsWomen?> GetProductAsync(int productId) =>
            await _context.CardsWoman.FindAsync(productId);

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await SaveAsync();
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId) =>
            await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
