using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface ICartService
    {
        Task AddToCartAsync(AddToCartDto dto);
        Task RemoveFromCartAsync(int cartItemId);
        Task<object> GetCartAsync();
    }
}