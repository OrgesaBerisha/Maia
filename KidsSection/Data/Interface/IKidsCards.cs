using KidsSection.Data.DTO;

namespace KidsSection.Data.Interface
{
    public interface IKidsCards
    {
        Task<IEnumerable<KidsCardsDto>> GetAllAsync();
        Task<IEnumerable<KidsCardsDto>> GetByCategoryAsync(int categoryId);
        Task<KidsCardsDto> CreateAsync(CreateKidsCardsDto dto);
        Task<bool> DeleteAsync(int id);
        Task<KidsCardsDto> UpdateAsync(int id, CreateKidsCardsDto dto);

    }
}
