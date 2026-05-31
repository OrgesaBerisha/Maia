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

        public async Task AddAsync(AddToWishlistDto dto)
        {
            var wishlist = await _context.Wishlists
                .Include(x => x.WishlistItems)
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                wishlist = new Wishlist();
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            var item = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = dto.ProductId
            };

            _context.WishlistItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var item = await _context.WishlistItems.FindAsync(id);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<object> GetAsync()
        {
            return await _context.Wishlists
                .Include(x => x.WishlistItems)
                .FirstOrDefaultAsync();
        }
    }
}