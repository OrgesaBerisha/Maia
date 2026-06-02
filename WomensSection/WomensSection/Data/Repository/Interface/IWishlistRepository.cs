using Maia.Models;

namespace Maia.Data.Repository.Interface
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetByUserIdAsync(int userId);
        Task<Wishlist>  CreateAsync(int userId);
        Task<bool>      ItemExistsAsync(int wishlistId, int productId);
        Task<WishlistItem?> GetItemByIdAsync(int itemId, int userId);
        Task AddItemAsync(WishlistItem item);
        Task RemoveItemAsync(WishlistItem item);
        Task SaveAsync();
    }
}
