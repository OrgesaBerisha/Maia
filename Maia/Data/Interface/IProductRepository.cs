using Maia.Models.NoSQL;

namespace Maia.Data.Interface;

public interface IProductRepository
{
    Task<IEnumerable<ProductDocument>> GetAllAsync();
    Task<ProductDocument?> GetByIdAsync(string id);
    Task<IEnumerable<ProductDocument>> GetByCategoryAsync(string category);
    Task<IEnumerable<ProductDocument>> SearchAsync(string query);
    Task<ProductDocument> CreateAsync(ProductDocument product);
    Task UpdateAsync(string id, ProductDocument product);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
