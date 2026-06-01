using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int userId, AddToCartDto dto)
        {
            // Merr cart të këtij useri, ose krijo të re
            var cart = await _context.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Nëse produkti ekziston tashmë, shto quantity
            var existing = cart.CartItems
                .FirstOrDefault(x => x.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            // Sigurohu që CartItem i takon këtij useri
            var item = await _context.CartItems
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(x => x.Id == cartItemId && x.Cart.UserId == userId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<object> GetCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
                return new { items = Array.Empty<object>(), total = 0 };

            var items = cart.CartItems.Select(i => new
            {
                i.Id,
                i.ProductId,
                ProductName = i.Product?.Title,
                ProductImage = i.Product?.ImageUrl,
                Price = i.Product?.Price,
                i.Quantity,
                Subtotal = i.Product?.Price * i.Quantity
            });

            return new
            {
                CartId = cart.Id,
                Items = items,
                Total = cart.CartItems.Sum(i => (i.Product?.Price ?? 0) * i.Quantity)
            };
        }
    }
}
