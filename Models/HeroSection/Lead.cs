using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NexsolCrmBackendVersion2.Models.HeroSection
{
    public class Lead
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact {  get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
