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
    }
}