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
            return await _context.KidsCards
                .Select(p => new KidsCardsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    KidsCategoryId = p.KidsCategoryId,
                    Description = p.Description
                })
                .ToListAsync();
        }

        // GET BY CATEGORY
        public async Task<IEnumerable<KidsCardsDto>> GetByCategoryAsync(int categoryId)
        {
            return await _context.KidsCards
                .Where(p => p.KidsCategoryId == categoryId)
                .Select(p => new KidsCardsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    KidsCategoryId = p.KidsCategoryId,
                    Description = p.Description
                })
                .ToListAsync();
        }

        // CREATE
        public async Task<KidsCardsDto> CreateAsync(CreateKidsCardsDto dto)
        {
            var card = new KidsCards
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                KidsCategoryId = dto.KidsCategoryId,
                Description = dto.Description
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
                Description = card.Description
            };
        }

        // UPDATE
        public async Task<KidsCardsDto> UpdateAsync(int id, CreateKidsCardsDto dto)
        {
            var entity = await _context.KidsCards.FindAsync(id);

            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.ImageUrl = dto.ImageUrl;
            entity.Price = dto.Price;
            entity.KidsCategoryId = dto.KidsCategoryId;
            entity.Description = dto.Description;

            await _context.SaveChangesAsync();

            return new KidsCardsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                KidsCategoryId = entity.KidsCategoryId,
                Description = entity.Description
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
    }
}