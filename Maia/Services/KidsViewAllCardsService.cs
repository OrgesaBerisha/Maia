using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;


namespace Maia.Services
{
    public class KidsViewAllCardsService : IKidsViewAllCards
    {
        private readonly DataContext _context;

        public KidsViewAllCardsService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KidsViewAllCardsDto>> GetAllAsync()
        {
            return await _context.KidsViewAllCards
                .Select(p => new KidsViewAllCardsDto
                {
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Category = p.Category,
                    Description = p.Description
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<KidsViewAllCardsDto>> GetByCategoryAsync(string category)
        {
            return await _context.KidsViewAllCards
                .Where(p => p.Category == category)
                .Select(p => new KidsViewAllCardsDto
                {
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Category = p.Category,
                    Description = p.Description
                })
                .ToListAsync();
        }

        public async Task<KidsViewAllCardsDto> CreateAsync(CreateKidsViewAllCardsDto dto)
        {
            var card = new KidsViewAllCards
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Category = dto.Category,
                Description = dto.Description
            };

            _context.KidsViewAllCards.Add(card);
            await _context.SaveChangesAsync();

            return new KidsViewAllCardsDto
            {
                Title = card.Title,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                Category = card.Category,
                Description = card.Description
            };
        }
    }
}
