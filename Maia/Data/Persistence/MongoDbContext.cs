using MongoDB.Driver;
using Maia.Models.NoSQL;

namespace Maia.Data.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
            ?? configuration["MongoDB:ConnectionString"]!;

        var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
            ?? configuration["MongoDB:DatabaseName"]!;

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);

        CreateIndexes();
    }

    public IMongoCollection<ProductDocument> Products =>
        _database.GetCollection<ProductDocument>("products");

    public IMongoCollection<AuditLogDocument> AuditLogs =>
        _database.GetCollection<AuditLogDocument>("audit_logs");

    private void CreateIndexes()
    {
        var productIndexKeys = Builders<ProductDocument>.IndexKeys
            .Ascending(p => p.Category)
            .Ascending(p => p.IsActive);
        Products.Indexes.CreateOne(new CreateIndexModel<ProductDocument>(productIndexKeys));

        var auditIndexKeys = Builders<AuditLogDocument>.IndexKeys
            .Descending(a => a.Timestamp)
            .Ascending(a => a.UserId);
        AuditLogs.Indexes.CreateOne(new CreateIndexModel<AuditLogDocument>(auditIndexKeys));
    }
}
