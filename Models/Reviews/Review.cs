using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NexsolCrmBackendVersion2.Models.Reviews
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
