using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.EntityFrameworkCore;
using Maia.Models;

namespace Maia.Services;

public class CardsWomenService : ICardsWomenService
{
    private readonly DataContext _context;

    public CardsWomenService(DataContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<IEnumerable<CardsWomenDto>> GetAllAsync()
    {
        return await _context.CardsWoman
            .Include(x => x.WomanCategory)
            .Select(x => new CardsWomenDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Category = x.WomanCategory.Name
            })
            .ToListAsync();
    }

    // GET BY CATEGORY
    public async Task<IEnumerable<CardsWomenDto>> GetByCategoryAsync(string category)
    {
        return await _context.CardsWoman
            .Include(x => x.WomanCategory)
            .Where(x => x.WomanCategory.Name == category)
            .Select(x => new CardsWomenDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Category = x.WomanCategory.Name
            })
            .ToListAsync();
    }

    // CREATE
    public async Task<CardsWomenDto> CreateAsync(CreateCardsWomenDto dto)
    {
        var product = new CardsWomen
        {
            Title = dto.Title,
            ImageUrl = dto.ImageUrl,
            Price = dto.Price,
            Description = dto.Description,
            WomanCategoryId = dto.WomanCategoryId
        };

        _context.CardsWoman.Add(product);
        await _context.SaveChangesAsync();

        return new CardsWomenDto
        {
            Id = product.Id,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Category = product.WomanCategory?.Name
        };
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.CardsWoman.FindAsync(id);

        if (product == null)
            return false;

        _context.CardsWoman.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    // UPDATE
    public async Task<CardsWomenDto?> UpdateAsync(int id, CreateCardsWomenDto dto)
    {
        var product = await _context.CardsWoman.FindAsync(id);

        if (product == null)
            return null;

        product.Title = dto.Title;
        product.ImageUrl = dto.ImageUrl;
        product.Price = dto.Price;
        product.Description = dto.Description;
        product.WomanCategoryId = dto.WomanCategoryId;

        await _context.SaveChangesAsync();

        return new CardsWomenDto
        {
            Id = product.Id,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Category = product.WomanCategory?.Name
        };
    }

    // 🚀 BROWSE FINAL (SEARCH + FILTER + SORT + PAGINATION)
    public async Task<PagedResult<CardsWomenDto>> BrowseAsync(
        string? search,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        string? sortBy,
        int page,
        int pageSize)
    {
        var query = _context.CardsWoman
            .Include(x => x.WomanCategory)
            .AsQueryable();

        // 🔎 SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                EF.Functions.Like(x.Title, $"%{search}%"));
        }

        // 🎯 FILTER
        if (categoryId.HasValue)
            query = query.Where(x => x.WomanCategoryId == categoryId.Value);

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice.Value);

        // 🔃 SORT
        query = sortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(x => x.Price),
            "price_desc" => query.OrderByDescending(x => x.Price),
            "newest" => query.OrderByDescending(x => x.Id),
            "a_z" => query.OrderBy(x => x.Title),
            "z_a" => query.OrderByDescending(x => x.Title),
            _ => query.OrderBy(x => x.Id)
        };

        // 📊 TOTAL ITEMS
        var totalItems = await query.CountAsync();

        // 📄 PAGINATION
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CardsWomenDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Category = x.WomanCategory.Name
            })
            .ToListAsync();

        return new PagedResult<CardsWomenDto>
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };
    }
}
