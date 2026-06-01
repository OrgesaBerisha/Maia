using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto dto);
    }
}