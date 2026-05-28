using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IKidsViewAllCards
    {
        Task<IEnumerable<KidsViewAllCardsDto>> GetAllAsync();
        Task<IEnumerable<KidsViewAllCardsDto>> GetByCategoryAsync(int categoryId);
        Task<KidsViewAllCardsDto> CreateAsync(CreateKidsViewAllCardsDto dto);
        Task<bool> DeleteAsync(int id);
        Task<KidsViewAllCardsDto> UpdateAsync(int id, CreateKidsViewAllCardsDto dto);
    }
}
