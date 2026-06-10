using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Data.Repository.Interface;
using Maia.Models;

namespace Maia.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _repo;

        public WishlistService(IWishlistRepository repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(int userId, AddToWishlistDto dto)
        {
            var wishlist = await _repo.GetByUserIdAsync(userId)
                        ?? await _repo.CreateAsync(userId);

            var source = string.IsNullOrEmpty(dto.Source) ? "WOMAN" : dto.Source;

            if (!await _repo.ItemExistsAsync(wishlist.Id, dto.ProductId, source))
            {
                await _repo.AddItemAsync(new WishlistItem
                {
                    WishlistId   = wishlist.Id,
                    ProductId    = dto.ProductId,
                    Source       = source,
                    ProductName  = dto.ProductName,
                    ProductImage = dto.ProductImage,
                    ProductPrice = dto.ProductPrice
                });
            }
        }

        public async Task RemoveAsync(int userId, int id)
        {
            var item = await _repo.GetItemByIdAsync(id, userId);
            if (item != null)
                await _repo.RemoveItemAsync(item);
        }

        public async Task<object> GetAsync(int userId)
        {
            var wishlist = await _repo.GetByUserIdAsync(userId);
            if (wishlist == null)
                return new { items = Array.Empty<object>() };

            var items = wishlist.WishlistItems.Select(i => new
            {
                i.Id,
                i.ProductId,
                Source       = i.Source ?? "WOMAN",
                ProductName  = i.ProductName,
                ProductImage = i.ProductImage,
                Price        = i.ProductPrice
            });

            return new { WishlistId = wishlist.Id, Items = items };
        }
    }
}
