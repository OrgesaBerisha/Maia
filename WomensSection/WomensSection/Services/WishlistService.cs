using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly DataContext _context;

        public WishlistService(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(int userId, AddToWishlistDto dto)
        {
            var wishlist = await _context.Wishlists
                .Include(x => x.WishlistItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userId };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            // Mos shto dy herë të njëjtin produkt
            var alreadyAdded = wishlist.WishlistItems
                .Any(x => x.ProductId == dto.ProductId);

            if (!alreadyAdded)
            {
                _context.WishlistItems.Add(new WishlistItem
                {
                    WishlistId = wishlist.Id,
                    ProductId = dto.ProductId
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(int userId, int id)
        {
            // Sigurohu që WishlistItem i takon këtij useri
            var item = await _context.WishlistItems
                .Include(x => x.Wishlist)
                .FirstOrDefaultAsync(x => x.Id == id && x.Wishlist.UserId == userId);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<object> GetAsync(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(x => x.WishlistItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (wishlist == null)
                return new { items = Array.Empty<object>() };

            var items = wishlist.WishlistItems.Select(i => new
            {
                i.Id,
                i.ProductId,
                ProductName = i.Product?.Title,
                ProductImage = i.Product?.ImageUrl,
                Price = i.Product?.Price
            });

            return new { WishlistId = wishlist.Id, Items = items };
        }
    }
}