using Maia.Models.NoSQL;

namespace Maia.Data.Interface;

public interface IAuditLogRepository
{
    Task LogAsync(AuditLogDocument log);
    Task<IEnumerable<AuditLogDocument>> GetByUserAsync(int userId, int limit = 50);
    Task<IEnumerable<AuditLogDocument>> GetByEntityAsync(string entity, string entityId);
    Task<IEnumerable<AuditLogDocument>> GetRecentAsync(int limit = 100);
}
