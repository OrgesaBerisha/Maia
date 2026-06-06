using Maia.Data.DTO;
using Maia.Data.Repository.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Data.Repository
{
    public class CardsWomenRepository : ICardsWomenRepository
    {
        private readonly DataContext _context;

        public CardsWomenRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardsWomen>> GetAllAsync() =>
            await _context.CardsWoman.Include(x => x.WomanCategory).ToListAsync();

        public async Task<IEnumerable<CardsWomen>> GetByCategoryAsync(string category) =>
            await _context.CardsWoman
                .Include(x => x.WomanCategory)
                .Where(x => x.WomanCategory != null && x.WomanCategory.Name == category)
                .ToListAsync();

        public async Task<CardsWomen?> GetByIdAsync(int id) =>
            await _context.CardsWoman.FindAsync(id);

        public async Task<CardsWomen> AddAsync(CardsWomen product)
        {
            _context.CardsWoman.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<CardsWomen?> UpdateAsync(int id, CardsWomen updated)
        {
            var product = await _context.CardsWoman.FindAsync(id);
            if (product == null) return null;

            product.Title           = updated.Title;
            product.ImageUrl        = updated.ImageUrl;
            product.Price           = updated.Price;
            product.Description     = updated.Description;
            product.WomanCategoryId = updated.WomanCategoryId;
            product.Color           = updated.Color;
            product.DiscountPercent = updated.DiscountPercent;
            product.UpdatedAt       = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<CardsWomenDto?> SetDiscountAsync(int id, int? discountPercent)
        {
            var product = await _context.CardsWoman
                .Include(x => x.WomanCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return null;

            product.DiscountPercent = discountPercent;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new CardsWomenDto
            {
                Id              = product.Id,
                Title           = product.Title,
                ImageUrl        = product.ImageUrl,
                Price           = product.Price,
                WomanCategoryId = product.WomanCategoryId,
                Category        = product.WomanCategory?.Name ?? string.Empty,
                Description     = product.Description,
                Color           = product.Color,
                DiscountPercent = product.DiscountPercent
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.CardsWoman.FindAsync(id);
            if (product == null) return false;

            _context.CardsWoman.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<CardsWomen> Items, int Total)> BrowseAsync(
            string? search, int? categoryId, decimal? minPrice,
            decimal? maxPrice, string? sortBy, int page, int pageSize)
        {
            var query = _context.CardsWoman
                .Include(x => x.WomanCategory)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{search}%"));

            if (categoryId.HasValue)
                query = query.Where(x => x.WomanCategoryId == categoryId.Value);

            if (minPrice.HasValue)
                query = query.Where(x => x.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(x => x.Price <= maxPrice.Value);

            query = sortBy?.ToLower() switch
            {
                "price_asc"  => query.OrderBy(x => x.Price),
                "price_desc" => query.OrderByDescending(x => x.Price),
                "newest"     => query.OrderByDescending(x => x.Id),
                "a_z"        => query.OrderBy(x => x.Title),
                "z_a"        => query.OrderByDescending(x => x.Title),
                _            => query.OrderBy(x => x.Id)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
