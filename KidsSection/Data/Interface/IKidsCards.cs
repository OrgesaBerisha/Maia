using KidsSection.Data.DTO;
using KidsSection.Models;

namespace KidsSection.Data.Interface
{
    public interface IKidsCards
    {
        Task<IEnumerable<KidsCardsDto>> GetAllAsync();
        Task<IEnumerable<KidsCardsDto>> GetByCategoryAsync(int categoryId);
        Task<KidsCardsDto> CreateAsync(CreateKidsCardsDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<KidsCardsDto>> SearchAsync(string name);
        Task<IEnumerable<KidsCardsDto>> FilterAsync(string? name, int? categoryId, decimal? min, decimal? max);
        Task<IEnumerable<KidsCardsDto>> SortAsync(SortOptions sortBy);
        Task<KidsCardsDto> UpdateAsync(int id, CreateKidsCardsDto dto);
        Task<KidsCardsDto?> SetDiscountAsync(int id, int? discountPercent);
    }
}
