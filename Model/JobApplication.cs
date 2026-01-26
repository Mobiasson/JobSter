using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JobSter.Model {
    public class JobApplication {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("companyId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CompanyId { get; set; }

        [BsonElement("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [BsonElement("appliedAt")]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; set; } = "Pending";

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }
    }
}

