using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maia.Models.NoSQL;

public class ProductDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("category")]
    public string Category { get; set; } = string.Empty;

    [BsonElement("stock")]
    public int Stock { get; set; }

    // Flexible attributes — arsyeja pse MongoDB (laptop ka RAM/CPU, këpucë ka madhësi/ngjyrë)
    [BsonElement("attributes")]
    public Dictionary<string, object> Attributes { get; set; } = new();

    [BsonElement("images")]
    public List<string> Images { get; set; } = new();

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("createdBy")]
    public string? CreatedBy { get; set; }

    [BsonElement("updatedBy")]
    public string? UpdatedBy { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
