using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IWishlistService
    {
        Task AddAsync(int userId, AddToWishlistDto dto);
        Task RemoveAsync(int userId, int id);
        Task<object> GetAsync(int userId);
    }
}
