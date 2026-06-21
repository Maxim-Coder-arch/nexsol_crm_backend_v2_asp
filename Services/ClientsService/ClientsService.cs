using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.Clients;
using NexsolCrmBackendVersion2.Models.Clients;
using System.Net;

namespace NexsolCrmBackendVersion2.Services.ClientsService
{
    public class ClientsService
    {
        public async Task<List<Client>> GetAllClientsService(IMongoCollection<Client> _clients)
        {
            return await _clients.Find(_ => true).ToListAsync();
        }

        public async Task<Client> AddNewClientService(IMongoCollection<Client> _clients, ClientDto model)
        {
            var newClient = new Client
            {
                Name = model.Name,
                WorkStatus = model.WorkStatus,
                PhysicalStatus = model.PhisicalStatus,
                Comment = model.Comment,
                AdditionalData = model.AdditionalData,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _clients.InsertOneAsync(newClient);

            return newClient;
        }

        public async Task<Client?> UpdateOneClientService(string id, ClientDto newClient, IMongoCollection<Client> _clients)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(c => c._id, id);

                var update = Builders<Client>.Update
                    .Set(c => c.Name, newClient.Name)
                    .Set(c => c.WorkStatus, newClient.WorkStatus)
                    .Set(c => c.PhysicalStatus, newClient.PhisicalStatus)
                    .Set(c => c.Comment, newClient.Comment ?? "")
                    .Set(c => c.AdditionalData, newClient.AdditionalData)
                    .Set(c => c.UpdatedAt, DateTime.UtcNow);

                var result = await _clients.UpdateOneAsync(filter, update);

                var updatedClient = await _clients.Find(filter).FirstOrDefaultAsync();
                return updatedClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteOneClientService(string id, IMongoCollection<Client> _clients)
        {
            var filter = Builders<Client>.Filter.Eq(c => c._id, id);
            var result = await _clients.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
