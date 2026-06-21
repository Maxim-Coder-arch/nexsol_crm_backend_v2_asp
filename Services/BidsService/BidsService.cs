using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.HeroSectionDtos;
using NexsolCrmBackendVersion2.Models.HeroSection;

namespace NexsolCrmBackendVersion2.Services.BidsService
{
    public class BidsService
    {
        public async Task<List<Lead>> GetAllLeadsService(IMongoCollection<Lead> _leads)
        {
            return await _leads.Find(_ => true).ToListAsync();
        }

        public async Task<Lead?> UpdateStatusOneLeadService(string id, IMongoCollection<Lead> _leads, LeadDto newLead)
        {
            var filter = Builders<Lead>.Filter.Eq(l => l.Id, id);
            var updateLead = Builders<Lead>.Update.Set(l => l.Status, newLead.Status).Set(l => l.UpdatedAt, DateTime.UtcNow);
            var result = await _leads.UpdateOneAsync(filter, updateLead);

            if (result.MatchedCount == 0)
                return null;

            return await _leads.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteOneService(string id, IMongoCollection<Lead> _leads)
        {
            var filter = Builders<Lead>.Filter.Eq(l => l.Id, id);
            var result = await _leads.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
