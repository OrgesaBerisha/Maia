using Maia.Models;

namespace Maia.Data.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<CardsWomen?> GetProductAsync(int productId);
        Task              AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task SaveAsync();
    }
}
