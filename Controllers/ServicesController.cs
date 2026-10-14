using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.ServicesDtos;
using NexsolCrmBackendVersion2.Models.Services;
using NexsolCrmBackendVersion2.Services.LinkServices;

namespace NexsolCrmBackendVersion2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IMongoCollection<Service> _services;
        private readonly LinkService _service;

        public ServicesController(IMongoDatabase database)
        {
            _services = database.GetCollection<Service>("services");
            _service = new();
        }

        #region GET
        [HttpGet("all")]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _service.GetAllServices(_services);
            return Ok(services);
        }

        #endregion

        #region POST

        [HttpPost("create")]
        public async Task<IActionResult> CreateOneService([FromBody] ServiceDto model)
        {
            var newService = await _service.CreateOneService(_services, model);

            if (newService == null)
                return BadRequest();
            
            return Ok(newService);
        }

        #endregion


        #region DELETE

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOneService(string id)
        {
            var result = await _service.DeleteOneService(_services, id);
            if (!result)
                return BadRequest();

            return Ok(new { success = true });
        }

        #endregion
    }
}
