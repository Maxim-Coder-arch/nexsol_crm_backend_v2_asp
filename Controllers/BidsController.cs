using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.HeroSectionDtos;
using NexsolCrmBackendVersion2.Models.HeroSection;
using NexsolCrmBackendVersion2.Services.BidsService;

namespace NexsolCrmBackendVersion2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly BidsService _bidsService;
        private readonly IMongoCollection<Lead> _leads;

        public BidsController(IMongoDatabase database)
        {
            _bidsService = new();
            _leads = database.GetCollection<Lead>("leads");
        }

        #region GET

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLeads()
        {
            var leads = await _bidsService.GetAllLeadsService(_leads);
            return Ok(leads);
        }

        #endregion

        #region PATCH

        [HttpPatch("bid/{id}")]
        public async Task<IActionResult> UpdateStatusOneLead([FromBody] LeadDto newLead,string id)
        {
            var lead = await _bidsService.UpdateStatusOneLeadService(id, _leads, newLead);
            if (lead == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(lead);
        }

        #endregion

        #region DELETE

        [HttpDelete("bid/{id}")]
        public async Task<IActionResult> DeleteOne(string id)
        {
            var deleted = await _bidsService.DeleteOneService(id, _leads);

            if (!deleted)
                return NotFound();
            return Ok(new { success = true });
        }

        #endregion
    }
}
