using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IKidsViewAllCards
    {
        Task<IEnumerable<KidsViewAllCardsDto>> GetAllAsync();
        Task<IEnumerable<KidsViewAllCardsDto>> GetByCategoryAsync(string category);
        Task<KidsViewAllCardsDto> CreateAsync(CreateKidsViewAllCardsDto dto);
    }
}
