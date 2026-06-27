namespace NexsolCrmBackendVersion2.Dtos.TeamMemberDtos
{
    public class TeamMemberDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<string> Specialties { get; set; }
        public List<string> Responsibilities { get; set; }
    }
}
