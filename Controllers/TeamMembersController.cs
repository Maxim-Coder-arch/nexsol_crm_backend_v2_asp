using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Models.TeamMembers;

namespace NexsolCrmBackendVersion2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamMembersController : ControllerBase
    {
        private readonly IMongoCollection<TeamMember> _teamMembers;

        public TeamMembersController(IMongoDatabase database)
        {
            _teamMembers = database.GetCollection<TeamMember>("users");
        }

        #region GET

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTeamMembers()
        {
            var members = await _teamMembers.Find(_ => true).ToListAsync();
            return Ok(members);
        }

        #endregion
    }
}
