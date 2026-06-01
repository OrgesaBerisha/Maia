using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, AddToCartDto dto);
        Task RemoveFromCartAsync(int userId, int cartItemId);
        Task<object> GetCartAsync(int userId);
    }
}