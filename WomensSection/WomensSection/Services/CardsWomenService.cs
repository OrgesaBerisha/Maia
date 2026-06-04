using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Data.Repository.Interface;
using Maia.Models;

namespace Maia.Services;

public class CardsWomenService : ICardsWomenService
{
    private readonly ICardsWomenRepository _repo;

    public CardsWomenService(ICardsWomenRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CardsWomenDto>> GetAllAsync()
    {
        var products = await _repo.GetAllAsync();
        return products.Select(ToDto);
    }

    public async Task<IEnumerable<CardsWomenDto>> GetByCategoryAsync(string category)
    {
        var products = await _repo.GetByCategoryAsync(category);
        return products.Select(ToDto);
    }

    public async Task<CardsWomenDto> CreateAsync(CreateCardsWomenDto dto)
    {
        var product = new CardsWomen
        {
            Title           = dto.Title,
            ImageUrl        = dto.ImageUrl ?? string.Empty,
            Price           = dto.Price,
            Description     = dto.Description,
            WomanCategoryId = dto.WomanCategoryId,
            Color           = dto.Color
        };
        var created = await _repo.AddAsync(product);
        return ToDto(created);
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _repo.DeleteAsync(id);

    public async Task<CardsWomenDto?> UpdateAsync(int id, CreateCardsWomenDto dto)
    {
        var updated = new CardsWomen
        {
            Title           = dto.Title,
            ImageUrl        = dto.ImageUrl ?? string.Empty,
            Price           = dto.Price,
            Description     = dto.Description,
            WomanCategoryId = dto.WomanCategoryId,
            Color           = dto.Color
        };
        var result = await _repo.UpdateAsync(id, updated);
        return result == null ? null : ToDto(result);
    }

    public async Task<PagedResult<CardsWomenDto>> BrowseAsync(
        string? search, int? categoryId, decimal? minPrice,
        decimal? maxPrice, string? sortBy, int page, int pageSize)
    {
        var (items, total) = await _repo.BrowseAsync(
            search, categoryId, minPrice, maxPrice, sortBy, page, pageSize);

        return new PagedResult<CardsWomenDto>
        {
            Items      = items.Select(ToDto),
            TotalItems = total,
            Page       = page,
            PageSize   = pageSize
        };
    }

    private static CardsWomenDto ToDto(CardsWomen x) => new()
    {
        Id              = x.Id,
        Title           = x.Title,
        ImageUrl        = x.ImageUrl,
        Price           = x.Price,
        WomanCategoryId = x.WomanCategoryId,
        Category        = x.WomanCategory?.Name ?? string.Empty,
        Description     = x.Description,
        Color           = x.Color
    };
}
