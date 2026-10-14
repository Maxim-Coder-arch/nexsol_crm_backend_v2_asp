using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection.Metadata;

namespace NexsolCrmBackendVersion2.Models.Services
{
    public class Service
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
