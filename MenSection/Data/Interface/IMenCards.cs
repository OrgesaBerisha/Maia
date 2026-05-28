using MenSection.Data.DTO;
using MenSection.Models;

namespace MenSection.Data.Interface
{
    public interface IMenCards
    {
        Task<IEnumerable<MenCardsDto>> GetAllAsync();
        Task<IEnumerable<MenCardsDto>> GetByCategoryAsync(int categoryId);
        Task<MenCardsDto> CreateAsync(CreateMenCardsDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<MenCardsDto>> SearchAsync(string name);
        Task<IEnumerable<MenCardsDto>> FilterAsync(string? name, int? categoryId, decimal? min, decimal? max);
        Task<IEnumerable<MenCardsDto>> SortAsync(SortOptions sortBy);
        Task<MenCardsDto> UpdateAsync(int id, CreateMenCardsDto dto);

    }
}
