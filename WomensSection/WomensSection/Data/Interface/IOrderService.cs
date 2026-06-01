using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int userId, CreateOrderDto dto);
        Task<object> GetOrdersAsync(int userId);
    }
}
