using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Models.AuthDtos;
using NexsolCrmBackendVersion2.Models.TeamMembers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NexsolCrmBackendVersion2.Services.Auth
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<TeamMember> _users;

        public AuthService(IConfiguration configuration, IMongoDatabase database)
        {
            _configuration = configuration;
            _users = database.GetCollection<TeamMember>("users");
        }

        public async Task<ServiceResult> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return ServiceResult.Fail("User with this email already exists");
            }

            var newUser = new TeamMember
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                Role = dto.Role ?? "viewer",
                Specialties = dto.Specialties ?? new List<string>(),
                Responsibilities = dto.Responsibilities ?? new List<string>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _users.InsertOneAsync(newUser);

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return ServiceResult<AuthResponseDto>.Fail("Invalid email or password");
            }

            if (!VerifyPassword(dto.Password, user.Password))
            {
                return ServiceResult<AuthResponseDto>.Fail("Invalid email or password");
            }

            var token = GenerateJwtToken(user._Id, user.Email, user.Name, user.Role);

            var response = new AuthResponseDto
            {
                Token = token,
                _Id = user._Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Specialties = user.Specialties ?? new List<string>(),
                Responsibilities = user.Responsibilities ?? new List<string>(),
                CreatedAt = user.CreatedAt
            };

            return ServiceResult<AuthResponseDto>.Ok(response);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string GenerateJwtToken(string userId, string email, string name, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? string.Empty;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult Ok() => new() { Success = true };
        public static ServiceResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static ServiceResult<T> Fail(string message) => new() { Success = false, ErrorMessage = message };
    }
}