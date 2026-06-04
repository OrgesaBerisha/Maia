using MenSection.Data;
using MenSection.Models;
using MenSection.Data.DTO;
using MenSection.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace MenSection.Services
{

    public class MenCardsService : IMenCards
    {
        private readonly DataContext _context;

        public MenCardsService(DataContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<IEnumerable<MenCardsDto>> GetAllAsync()
        {
            var data = await _context.MenCards
                .Include(p => p.MenCategory)
                .ToListAsync();

            return data.Select(p => new MenCardsDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                Price = p.Price,

                MenCategoryId = p.MenCategoryId,
                MenCategoryName = p.MenCategory?.Name,

                Description = p.Description
            });
        }

        // GET BY CATEGORY
        public async Task<IEnumerable<MenCardsDto>> GetByCategoryAsync(int categoryId)
        {
            var data = await _context.MenCards
                .Where(p => p.MenCategoryId == categoryId)
                .Include(p => p.MenCategory)
                .ToListAsync();

            return data.Select(p => new MenCardsDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                Price = p.Price,

                MenCategoryId = p.MenCategoryId,
                MenCategoryName = p.MenCategory?.Name,



                Description = p.Description
            });
        }

        // CREATE
        public async Task<MenCardsDto> CreateAsync(CreateMenCardsDto dto)
        {
            var card = new MenCards
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                MenCategoryId = dto.MenCategoryId,
                Description = dto.Description
            };

            _context.MenCards.Add(card);
            await _context.SaveChangesAsync();

            return new MenCardsDto
            {
                Id = card.Id,
                Title = card.Title,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                MenCategoryId = card.MenCategoryId,
                Description = card.Description
            };
        }

        // UPDATE
        public async Task<MenCardsDto> UpdateAsync(int id, CreateMenCardsDto dto)
        {
            var entity = await _context.MenCards.FindAsync(id);

            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.ImageUrl = dto.ImageUrl;
            entity.Price = dto.Price;
            entity.MenCategoryId = dto.MenCategoryId;
            entity.Description = dto.Description;

            await _context.SaveChangesAsync();

            return new MenCardsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                MenCategoryId = entity.MenCategoryId,
                Description = entity.Description
            };
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.MenCards.FindAsync(id);

            if (entity == null) return false;

            _context.MenCards.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // SEARCH
        public async Task<IEnumerable<MenCardsDto>> SearchAsync(string name)
        {
            var data = await _context.MenCards
                .Where(x => x.Title.ToLower().Contains(name.ToLower()))
                .Include(x => x.MenCategory)

                .ToListAsync();

            return data.Select(x => new MenCardsDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,

                MenCategoryId = x.MenCategoryId,
                MenCategoryName = x.MenCategory?.Name,


                Description = x.Description
            });
        }

        // FILTER
        public async Task<IEnumerable<MenCardsDto>> FilterAsync(string? name, int? categoryId, decimal? min, decimal? max)
        {
            var query = _context.MenCards
                .Include(x => x.MenCategory)

                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Title.Contains(name));

            if (categoryId.HasValue)
                query = query.Where(x => x.MenCategoryId == categoryId);

            if (min.HasValue)
                query = query.Where(x => x.Price >= min);

            if (max.HasValue)
                query = query.Where(x => x.Price <= max);

            var data = await query.ToListAsync();

            return data.Select(x => new MenCardsDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,

                MenCategoryId = x.MenCategoryId,
                MenCategoryName = x.MenCategory?.Name,


                Description = x.Description
            });
        }

        // SORT
        public async Task<IEnumerable<MenCardsDto>> SortAsync(SortOptions sortBy)
        {
            var query = _context.MenCards
                .Include(x => x.MenCategory)

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

            return data.Select(x => new MenCardsDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Price = x.Price,

                MenCategoryId = x.MenCategoryId,
                MenCategoryName = x.MenCategory?.Name,


                Description = x.Description
            });
        }

    }

}