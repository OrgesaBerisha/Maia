using MongoDB.Driver;
using Maia.Models.NoSQL;
using Maia.Data.Interface;
using Maia.Data.Persistence;

namespace Maia.Data.Repository.NoSQL;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly IMongoCollection<AuditLogDocument> _auditLogs;

    public AuditLogRepository(MongoDbContext context)
    {
        _auditLogs = context.AuditLogs;
    }

    public async Task LogAsync(AuditLogDocument log)
    {
        log.Timestamp = DateTime.UtcNow;
        await _auditLogs.InsertOneAsync(log);
    }

    public async Task<IEnumerable<AuditLogDocument>> GetByUserAsync(int userId, int limit = 50)
    {
        return await _auditLogs
            .Find(a => a.UserId == userId)
            .SortByDescending(a => a.Timestamp)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLogDocument>> GetByEntityAsync(string entity, string entityId)
    {
        return await _auditLogs
            .Find(a => a.Entity == entity && a.EntityId == entityId)
            .SortByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLogDocument>> GetRecentAsync(int limit = 100)
    {
        return await _auditLogs
            .Find(_ => true)
            .SortByDescending(a => a.Timestamp)
            .Limit(limit)
            .ToListAsync();
    }
}
