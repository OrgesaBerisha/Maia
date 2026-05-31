using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IWishlistService
    {
        Task AddAsync(AddToWishlistDto dto);
        Task RemoveAsync(int id);
        Task<object> GetAsync();
    }
}