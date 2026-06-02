using Maia.Models;

namespace Maia.Data.Repository.Interface
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(int userId);
        Task<Cart>  CreateAsync(int userId);
        Task<CartItem?> GetItemAsync(int cartId, int productId);
        Task<CartItem?> GetItemByIdAsync(int cartItemId, int userId);
        Task AddItemAsync(CartItem item);
        Task RemoveItemAsync(CartItem item);
        Task SaveAsync();
    }
}
