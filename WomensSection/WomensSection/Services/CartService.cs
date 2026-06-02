using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Data.Repository.Interface;
using Maia.Models;

namespace Maia.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;

        public CartService(ICartRepository repo)
        {
            _repo = repo;
        }

        public async Task AddToCartAsync(int userId, AddToCartDto dto)
        {
            var cart = await _repo.GetByUserIdAsync(userId)
                    ?? await _repo.CreateAsync(userId);

            var existing = await _repo.GetItemAsync(cart.Id, dto.ProductId);
            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                await _repo.SaveAsync();
            }
            else
            {
                await _repo.AddItemAsync(new CartItem
                {
                    CartId    = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity  = dto.Quantity
                });
            }
        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            var item = await _repo.GetItemByIdAsync(cartItemId, userId);
            if (item != null)
                await _repo.RemoveItemAsync(item);
        }

        public async Task<object> GetCartAsync(int userId)
        {
            var cart = await _repo.GetByUserIdAsync(userId);
            if (cart == null)
                return new { items = Array.Empty<object>(), total = 0 };

            var items = cart.CartItems.Select(i => new
            {
                i.Id,
                i.ProductId,
                ProductName  = i.Product?.Title,
                ProductImage = i.Product?.ImageUrl,
                Price        = i.Product?.Price,
                i.Quantity,
                Subtotal     = i.Product?.Price * i.Quantity
            });

            return new
            {
                CartId = cart.Id,
                Items  = items,
                Total  = cart.CartItems.Sum(i => (i.Product?.Price ?? 0) * i.Quantity)
            };
        }
    }
}
