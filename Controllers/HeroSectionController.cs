using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Models.HeroSection;
using NexsolCrmBackendVersion2.Services.HeroSectionService;

namespace NexsolCrmBackendVersion2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroSectionController : ControllerBase
    {
        private readonly IMongoCollection<Visitor> _visitors;
        private readonly IMongoCollection<Lead> _leads;
        private readonly IMongoCollection<User> _users;
        private readonly HeroSectionLogicService _service;

        public HeroSectionController(IMongoDatabase database)
        {
            _visitors = database.GetCollection<Visitor>("visitors");
            _leads = database.GetCollection<Lead>("leads");
            _users = database.GetCollection<User>("users");
            _service = new HeroSectionLogicService();
        }

        #region get

        [HttpGet("visitors")]
        public async Task<IActionResult> GetAllVisitors()
        {
            var visitors = await _service.GetAllVisitorsService(_visitors);
            return Ok(visitors);
        }

        [HttpGet("leads")]
        public async Task<IActionResult> GetAllLeads()
        {
            var leads = await _service.GetAllLeadsService(_leads);
            return Ok(leads);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsersService(_users);
            return Ok(users);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetVisitorsStats()
        {
            var stats = await _service.GetVisitorsStatsService(_visitors);

            if (stats == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Failed to fetch statistics" });

            return Ok(stats);
        }

        [HttpGet("chart")]
        public async Task<IActionResult> GetChartData([FromQuery] string period = "week")
        {
            var chartData = await _service.GetChartDataService(period, _visitors);

            if (chartData == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Failed to fetch chart data" });

            return Ok(chartData);
        }

        [HttpGet("visitors/details")]
        public async Task<IActionResult> GetRecentVisitors()
        {
            var recentData = await _service.GetRecentVisitorsService(_visitors);

            if (recentData == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Failed to fetch visitors" });

            return Ok(recentData);
        }

        #endregion
    }
}