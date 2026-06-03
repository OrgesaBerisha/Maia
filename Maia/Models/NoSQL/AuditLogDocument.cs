using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maia.Models.NoSQL;

public class AuditLogDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("userId")]
    public int? UserId { get; set; }

    [BsonElement("action")]
    public string Action { get; set; } = string.Empty; // "CREATE", "UPDATE", "DELETE", "LOGIN"

    [BsonElement("entity")]
    public string Entity { get; set; } = string.Empty; // "Product", "Order", "User"

    [BsonElement("entityId")]
    public string? EntityId { get; set; }

    [BsonElement("oldValues")]
    public BsonDocument? OldValues { get; set; }

    [BsonElement("newValues")]
    public BsonDocument? NewValues { get; set; }

    [BsonElement("ipAddress")]
    public string? IpAddress { get; set; }

    [BsonElement("userAgent")]
    public string? UserAgent { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [BsonElement("createdBy")]
    public string? CreatedBy { get; set; }
}
