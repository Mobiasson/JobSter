using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSter.Model;

public class Company {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("companyName")]
    public string CompanyName { get; set; } = string.Empty;

    [BsonElement("contactPerson")]
    public string ContactPerson { get; set; } = string.Empty;

    [BsonElement("industry")]
    public string Industry { get; set; } = string.Empty;
}
