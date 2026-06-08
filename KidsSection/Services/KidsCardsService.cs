using KidsSection.Data;
using KidsSection.Data.DTO;
using KidsSection.Data.Interface;
using KidsSection.Models;
using Microsoft.EntityFrameworkCore;

namespace KidsSection.Services
{
    public class KidsCardsService : IKidsCards
    {
        private readonly DataContext _context;

        public KidsCardsService(DataContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<IEnumerable<KidsCardsDto>> GetAllAsync()
        {
            var data = await _context.KidsCards
                .Include(p => p.KidsCategory)
                .Include(p => p.KidsProductType)
                .ToListAsync();

            return data.Select(p => new KidsCardsDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                Price = p.Price,

                KidsCategoryId = p.KidsCategoryId,
                KidsCategoryName = p.KidsCategory?.Name,

                KidsProductTypeId = p.KidsProductTypeId,
                KidsProductTypeName = p.KidsProductType?.Name,

                Description = p.Description,
                DiscountPercent = p.DiscountPercent,
                Color = p.Color
            });
        }

        // GET BY CATEGORY
        public async Task<IEnumerable<KidsCardsDto>> GetByCategoryAsync(int categoryId)
        {
            var data = await _context.KidsCards
                .Where(p => p.KidsCategoryId == categoryId)
                .Include(p => p.KidsCategory)
                .Include(p => p.KidsProductType)
                .ToListAsync();

            return data.Select(p => new KidsCardsDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                Price = p.Price,

                KidsCategoryId = p.KidsCategoryId,
                KidsCategoryName = p.KidsCategory?.Name,

                KidsProductTypeId = p.KidsProductTypeId,
                KidsProductTypeName = p.KidsProductType?.Name,

                Description = p.Description,
                DiscountPercent = p.DiscountPercent,
                Color = p.Color
            });
        }

        // CREATE
        public async Task<KidsCardsDto> CreateAsync(CreateKidsCardsDto dto)
        {
            var card = new KidsCards
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl ?? string.Empty,
                Price = dto.Price,
                KidsCategoryId = dto.KidsCategoryId,
                KidsProductTypeId = dto.KidsProductTypeId,
                Description = dto.Description,
                DiscountPercent = dto.DiscountPercent,
                Color = dto.Color
            };

            _context.KidsCards.Add(card);
            await _context.SaveChangesAsync();

            return new KidsCardsDto
            {
                Id = card.Id,
                Title = card.Title,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                KidsCategoryId = card.KidsCategoryId,
                KidsProductTypeId = card.KidsProductTypeId,
                Description = card.Description,
                DiscountPercent = card.DiscountPercent
            };
        }

        // UPDATE
        public async Task<KidsCardsDto> UpdateAsync(int id, CreateKidsCardsDto dto)
        {
            var entity = await _context.KidsCards.FindAsync(id);

            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.ImageUrl = dto.ImageUrl ?? string.Empty;
            entity.Price = dto.Price;
            entity.KidsCategoryId = dto.KidsCategoryId;
            entity.KidsProductTypeId = dto.KidsProductTypeId;
            entity.Description = dto.Description;
            if (dto.DiscountPercent.HasValue)
                entity.DiscountPercent = dto.DiscountPercent;
            entity.Color = dto.Color;

            await _context.SaveChangesAsync();

            return new KidsCardsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                KidsCategoryId = entity.KidsCategoryId,
                KidsProductTypeId = entity.KidsProductTypeId,
                Description = entity.Description,
                DiscountPercent = entity.DiscountPercent
            };
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.KidsCards.FindAsync(id);

            if (entity == null) return false;

            _context.KidsCards.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // SEARCH
        public async Task<IEnumerable<KidsCardsDto>> SearchAsync(string name)
        {
            var data = await _context.KidsCards
                .Where(x => x.Title.ToLower().Contains(name.ToLower()))
                .Include(x => x.KidsCategory)
                .Include(x => x.KidsProductType)
                .ToListAsync();

            return data.Select(x => new KidsCardsDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,

                KidsCategoryId = x.KidsCategoryId,
                KidsCategoryName = x.KidsCategory?.Name,

                KidsProductTypeId = x.KidsProductTypeId,
                KidsProductTypeName = x.KidsProductType?.Name,

                Description = x.Description,
                DiscountPercent = x.DiscountPercent,
                Color = x.Color
            });
        }

        // FILTER
        public async Task<IEnumerable<KidsCardsDto>> FilterAsync(string? name, int? categoryId, decimal? min, decimal? max)
        {
            var query = _context.KidsCards
                .Include(x => x.KidsCategory)
                .Include(x => x.KidsProductType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Title.Contains(name));

            if (categoryId.HasValue)
                query = query.Where(x => x.KidsCategoryId == categoryId);

            if (min.HasValue)
                query = query.Where(x => x.Price >= min);

            if (max.HasValue)
                query = query.Where(x => x.Price <= max);

            var data = await query.ToListAsync();

            return data.Select(x => new KidsCardsDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,

                KidsCategoryId = x.KidsCategoryId,
                KidsCategoryName = x.KidsCategory?.Name,

                KidsProductTypeId = x.KidsProductTypeId,
                KidsProductTypeName = x.KidsProductType?.Name,

                Description = x.Description,
                DiscountPercent = x.DiscountPercent,
                Color = x.Color
            });
        }

        // SORT
        public async Task<IEnumerable<KidsCardsDto>> SortAsync(SortOptions sortBy)
        {
            var query = _context.KidsCards
                .Include(x => x.KidsCategory)
                .Include(x => x.KidsProductType)
                .AsQueryable();

            query = sortBy switch
            {
                SortOptions.price_asc => query.OrderBy(x => x.Price),
                SortOptions.price_desc => query.OrderByDescending(x => x.Price),
                SortOptions.name_asc => query.OrderBy(x => x.Title),
                SortOptions.name_desc => query.OrderByDescending(x => x.Title),
                _ => query
            };

            var data = await query.ToListAsync();

            return data.Select(x => new KidsCardsDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,

                KidsCategoryId = x.KidsCategoryId,
                KidsCategoryName = x.KidsCategory?.Name,

                KidsProductTypeId = x.KidsProductTypeId,
                KidsProductTypeName = x.KidsProductType?.Name,

                Description = x.Description,
                DiscountPercent = x.DiscountPercent,
                Color = x.Color
            });
        }

        public async Task<KidsCardsDto?> SetDiscountAsync(int id, int? discountPercent)
        {
            var entity = await _context.KidsCards.FindAsync(id);
            if (entity == null) return null;

            entity.DiscountPercent = discountPercent;
            await _context.SaveChangesAsync();

            return new KidsCardsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                KidsCategoryId = entity.KidsCategoryId,
                KidsProductTypeId = entity.KidsProductTypeId,
                Description = entity.Description,
                DiscountPercent = entity.DiscountPercent
            };
        }
    }
}