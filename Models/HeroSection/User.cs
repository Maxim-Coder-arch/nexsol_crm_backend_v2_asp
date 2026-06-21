using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NexsolCrmBackendVersion2.Models.HeroSection
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<string> Specialties { get; set; }
        public List<string> Responsibilities { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
