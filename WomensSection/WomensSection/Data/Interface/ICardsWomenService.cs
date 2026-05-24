using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface ICardsWomenService
    {
        Task<IEnumerable<CardsWomenDto>> GetAllAsync();

        Task<IEnumerable<CardsWomenDto>> GetByCategoryAsync(string category);

        Task<CardsWomenDto> CreateAsync(CreateCardsWomenDto dto);

        Task<bool> DeleteAsync(int id);

        Task<CardsWomenDto?> UpdateAsync(int id, CreateCardsWomenDto dto);

        // 🔥 FINAL: ZARA STYLE BROWSE (SEARCH + FILTER + SORT + PAGINATION)
        Task<IEnumerable<CardsWomenDto>> BrowseAsync(
            string? search,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            int page,
            int pageSize);
    }
}