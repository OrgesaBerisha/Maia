using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;




namespace Maia.Services
{
    public class CardsWomenService : ICardsWomenService

    {
        private readonly DataContext _context;

        public CardsWomenService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardsWomenDto>> GetAllAsync()
        {
            return await _context.CardsWoman
                .Select(p => new CardsWomenDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Category = p.Category
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CardsWomenDto>> GetByCategoryAsync(string category)
        {
            return await _context.CardsWoman
                .Where(p => p.Category == category)
                .Select(p => new CardsWomenDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Category = p.Category
                })
                .ToListAsync();
        }

        public async Task<CardsWomenDto> CreateAsync(CreateCardsWomenDto dto)
        {
            var product = new CardsWomen
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Category = dto.Category,
                Description = dto.Description
            };

            _context.CardsWoman.Add(product);
            await _context.SaveChangesAsync();

            return new CardsWomenDto
            {
                Id = product.Id,
                Title = product.Title,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Category = product.Category
            };
        }
    }
}
