using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.ServicesDtos;
using NexsolCrmBackendVersion2.Models.Services;

namespace NexsolCrmBackendVersion2.Services.LinkServices
{
    public class LinkService
    {
        public async Task<List<Service>> GetAllServices(IMongoCollection<Service> _services)
        {
            return await _services.Find(_ => true).ToListAsync();
        }

        public async Task<Service> CreateOneService(IMongoCollection<Service> _services, ServiceDto model)
        {
            var newService = new Service
            {
                Title = model.Title,
                Description = model.Description,
                Url = model.Url,
                CreatedAt = DateTime.UtcNow,
            };

            await _services.InsertOneAsync(newService);

            return newService;
        }

        public async Task<bool> DeleteOneService(IMongoCollection<Service> _services, string id)
        {
            var filter = Builders<Service>.Filter.Eq(s => s._Id, id);
            var result = await _services.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}
