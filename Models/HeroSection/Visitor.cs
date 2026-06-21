using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NexsolCrmBackendVersion2.Models.HeroSection
{
    public class Visitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string VisitorId { get; set; }
        public string Page {  get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
        public string DateKey { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
