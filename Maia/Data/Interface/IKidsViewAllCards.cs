using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IKidsViewAllCards
    {
        Task<IEnumerable<KidsViewAllCardsDto>> GetAllAsync();
        Task<IEnumerable<KidsViewAllCardsDto>> GetByCategoryAsync(string category);
        Task<KidsViewAllCardsDto> CreateAsync(CreateKidsViewAllCardsDto dto);
        Task<bool> DeleteAsync(int id);
        Task<KidsViewAllCardsDto> UpdateAsync(int id, CreateKidsViewAllCardsDto dto);
    }
}
