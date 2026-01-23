using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSter.Model;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("pinHash")]
    public string PinHash { get; set; } = string.Empty;

    [BsonElement("pinSalt")]
    public string PinSalt { get; set; } = string.Empty;
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
