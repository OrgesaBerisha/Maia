using Maia.Data.Repository.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Data.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly DataContext _context;

        public WishlistRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Wishlist?> GetByUserIdAsync(int userId) =>
            await _context.Wishlists
                .Include(x => x.WishlistItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<Wishlist> CreateAsync(int userId)
        {
            var wishlist = new Wishlist { UserId = userId };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> ItemExistsAsync(int wishlistId, int productId) =>
            await _context.WishlistItems
                .AnyAsync(x => x.WishlistId == wishlistId && x.ProductId == productId);

        public async Task<WishlistItem?> GetItemByIdAsync(int itemId, int userId) =>
            await _context.WishlistItems
                .Include(x => x.Wishlist)
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Wishlist.UserId == userId);

        public async Task AddItemAsync(WishlistItem item)
        {
            _context.WishlistItems.Add(item);
            await SaveAsync();
        }

        public async Task RemoveItemAsync(WishlistItem item)
        {
            _context.WishlistItems.Remove(item);
            await SaveAsync();
        }

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
