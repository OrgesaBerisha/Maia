using Maia.Data.DTO;
using Maia.Models;

namespace Maia.Data.Repository.Interface
{
    public interface ICardsWomenRepository
    {
        Task<IEnumerable<CardsWomen>> GetAllAsync();
        Task<IEnumerable<CardsWomen>> GetByCategoryAsync(string category);
        Task<CardsWomen?> GetByIdAsync(int id);
        Task<CardsWomen> AddAsync(CardsWomen product);
        Task<CardsWomen?> UpdateAsync(int id, CardsWomen updated);
        Task<bool> DeleteAsync(int id);
        Task<CardsWomenDto?> SetDiscountAsync(int id, int? discountPercent);
        Task<(IEnumerable<CardsWomen> Items, int Total)> BrowseAsync(
            string? search, int? categoryId, decimal? minPrice,
            decimal? maxPrice, string? sortBy, int page, int pageSize);
    }
}
