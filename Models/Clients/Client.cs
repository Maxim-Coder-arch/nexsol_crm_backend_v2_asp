using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NexsolCrmBackendVersion2.Models.Clients
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public string WorkStatus { get; set; }
        public string PhysicalStatus { get; set; }
        public string Comment { get; set; }
        public List<Dictionary<string, string>> AdditionalData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}