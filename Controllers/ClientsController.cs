using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.Clients;
using NexsolCrmBackendVersion2.Models.Clients;
using NexsolCrmBackendVersion2.Services.ClientsService;

namespace NexsolCrmBackendVersion2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsService _service;
        private readonly IMongoCollection<Client> _clients;

        public ClientsController(IMongoDatabase database)
        {
            _service = new();
            _clients = database.GetCollection<Client>("clients");
        }


        #region GET

        [HttpGet("all")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _service.GetAllClientsService(_clients);
            return Ok(clients);
        }

        #endregion

        #region POST

        [HttpPost("add")]
        public async Task<IActionResult> AddNewClient([FromBody] ClientDto model)
        {
            var newClient = await _service.AddNewClientService(_clients, model);
            return Ok(newClient);
        }

        #endregion

        #region PATCH

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateOneClient(string id, [FromBody] ClientDto newClient)
        {
            var updatedClient = await _service.UpdateOneClientService(id, newClient, _clients);
            if (updatedClient == null)
                return NotFound();

            return Ok(updatedClient);
        }

        #endregion

        #region DELETE

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOneClient(string id)
        {
            var deletedClient = await _service.DeleteOneClientService(id, _clients);

            if (!deletedClient)
                return NotFound();

            return Ok(new { success = true });
        }

        #endregion

    }
}
