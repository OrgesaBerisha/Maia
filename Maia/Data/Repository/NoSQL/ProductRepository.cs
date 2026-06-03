using MongoDB.Driver;
using Microsoft.Extensions.Caching.Memory;
using Maia.Models.NoSQL;
using Maia.Data.Interface;
using Maia.Data.Persistence;

namespace Maia.Data.Repository.NoSQL;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<ProductDocument> _products;
    private readonly IMemoryCache _cache;

    public ProductRepository(MongoDbContext context, IMemoryCache cache)
    {
        _products = context.Products;
        _cache = cache;
    }

    public async Task<IEnumerable<ProductDocument>> GetAllAsync()
    {
        return await _products
            .Find(p => p.IsActive)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<ProductDocument?> GetByIdAsync(string id)
    {
        var cacheKey = $"product:{id}";
        if (_cache.TryGetValue(cacheKey, out ProductDocument? cached))
            return cached;

        var product = await _products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (product != null)
            _cache.Set(cacheKey, product, TimeSpan.FromMinutes(10));

        return product;
    }

    public async Task<IEnumerable<ProductDocument>> GetByCategoryAsync(string category)
    {
        return await _products
            .Find(p => p.Category == category && p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductDocument>> SearchAsync(string query)
    {
        var filter = Builders<ProductDocument>.Filter.Or(
            Builders<ProductDocument>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(query, "i")),
            Builders<ProductDocument>.Filter.Regex(p => p.Description, new MongoDB.Bson.BsonRegularExpression(query, "i"))
        );

        return await _products.Find(filter).Limit(20).ToListAsync();
    }

    public async Task<ProductDocument> CreateAsync(ProductDocument product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        await _products.InsertOneAsync(product);
        return product;
    }

    public async Task UpdateAsync(string id, ProductDocument product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        await _products.ReplaceOneAsync(p => p.Id == id, product);
        _cache.Remove($"product:{id}");
    }

    public async Task DeleteAsync(string id)
    {
        var update = Builders<ProductDocument>.Update
            .Set(p => p.IsActive, false)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        await _products.UpdateOneAsync(p => p.Id == id, update);
        _cache.Remove($"product:{id}");
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var count = await _products.CountDocumentsAsync(p => p.Id == id);
        return count > 0;
    }
}
