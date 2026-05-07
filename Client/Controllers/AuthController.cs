using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var email = _configuration.GetSection("Authentication:Email").Value;
            var password = _configuration.GetSection("Authentication:Password").Value;

            if (request.Email != email || request.Password != password)
            {
                return Unauthorized("Credenciais inválidas");
            }

            var jwtSecret = _configuration.GetSection("Authentication:JwtSecret").Value;
            var accessExpiry = int.Parse(_configuration.GetSection("Authentication:AccessTokenExpiryMinutes").Value);
            var refreshExpiry = int.Parse(_configuration.GetSection("Authentication:RefreshTokenExpiryHours").Value);

            var accessToken = GenerateToken(jwtSecret, accessExpiry);
            var refreshToken = GenerateToken(jwtSecret, refreshExpiry * 60);
            var expiry = DateTime.UtcNow.AddMinutes(accessExpiry);

            InMemoryTokenStore.StoreTokens(accessToken, refreshToken, expiry);

            return Ok(new { accessToken, refreshToken, expiry });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            var tokens = InMemoryTokenStore.GetTokens();
            if (tokens == null || tokens.Value.Refresh != request.RefreshToken)
            {
                return Unauthorized("Refresh token inválido");
            }

            var jwtSecret = _configuration.GetSection("Authentication:JwtSecret").Value;
            var accessExpiry = int.Parse(_configuration.GetSection("Authentication:AccessTokenExpiryMinutes").Value);

            var newAccessToken = GenerateToken(jwtSecret, accessExpiry);
            var expiry = DateTime.UtcNow.AddMinutes(accessExpiry);

            InMemoryTokenStore.StoreTokens(newAccessToken, tokens.Value.Refresh, expiry);

            return Ok(new { accessToken = newAccessToken, expiry });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            InMemoryTokenStore.ClearTokens();
            return Ok("Logout realizado");
        }

        private string GenerateToken(string secret, int expiryMinutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
    }
}