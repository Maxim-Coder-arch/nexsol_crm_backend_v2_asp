namespace NexsolCrmBackendVersion2.Models.AuthDtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string _Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<string> Specialties { get; set; }
        public List<string> Responsibilities { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
