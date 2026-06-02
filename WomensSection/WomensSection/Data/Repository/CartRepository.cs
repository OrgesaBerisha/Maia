using Maia.Data.Repository.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Data.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByUserIdAsync(int userId) =>
            await _context.Carts
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<Cart> CreateAsync(int userId)
        {
            var cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItem?> GetItemAsync(int cartId, int productId) =>
            await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);

        public async Task<CartItem?> GetItemByIdAsync(int cartItemId, int userId) =>
            await _context.CartItems
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(x => x.Id == cartItemId && x.Cart.UserId == userId);

        public async Task AddItemAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await SaveAsync();
        }

        public async Task RemoveItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await SaveAsync();
        }

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
