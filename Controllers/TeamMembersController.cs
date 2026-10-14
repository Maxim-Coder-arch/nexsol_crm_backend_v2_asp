using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.TeamMemberDtos;
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

        #region POST

        [HttpPost("add")]
        public async Task<IActionResult> AddNewUser([FromBody] TeamMemberDto model)
        {
            var newUser = new TeamMember
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                Specialties = model.Specialties,
                Responsibilities = model.Responsibilities,
                UpdatedAt = DateTime.UtcNow,
            };

            await _teamMembers.InsertOneAsync(newUser);
            return Ok(newUser);
        }

        #endregion

        #region PATCH

        [HttpPatch("user/{id}")]
        public async Task<IActionResult> UpdateOneUser(string id, [FromBody] TeamMemberDto model)
        {

            try
            {
                var filter = Builders<TeamMember>.Filter.Eq(u => u._Id, id);
                var newUser = Builders<TeamMember>.Update
                    .Set(u => u.Name, model.Name)
                    .Set(u => u.Email, model.Email)
                    .Set(u => u.Password, model.Password)
                    .Set(u => u.Role, model.Role)
                    .Set(u => u.Specialties, model.Specialties)
                    .Set(u => u.Responsibilities, model.Responsibilities);
                var result = await _teamMembers.UpdateOneAsync(filter, newUser);

                if (result.MatchedCount == 0)
                    return BadRequest();

                return Ok(_teamMembers.Find(filter).FirstOrDefault());
            } 
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region DELETE

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteOneUser(string id)
        {
            var filter = Builders<TeamMember>.Filter.Eq(u => u._Id, id);

            if (filter == null)
                return BadRequest();

            var result = await _teamMembers.DeleteOneAsync(filter);
            return Ok(result.DeletedCount > 0);
        }

        #endregion
    }
}
