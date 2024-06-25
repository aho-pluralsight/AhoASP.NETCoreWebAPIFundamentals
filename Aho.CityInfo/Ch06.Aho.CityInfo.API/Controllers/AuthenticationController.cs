using Ch06.Aho.CityInfo.API.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ch06.Aho.CityInfo.API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authentication)
        {
            if (string.IsNullOrEmpty(authentication.UserName) || string.IsNullOrEmpty(authentication.Password))
            {
                return Unauthorized();
            }

            var user = ValidateUserCredentials(authentication.UserName, authentication.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(user);
            if (token.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        private CityInfoUser ValidateUserCredentials(string? login, string? password)
        {
            //[AHO] Pretending that the user has provided valid credentials
            return new CityInfoUser("AHO123456", login ?? "mad", "Mad", "HR", "Dev");
        }

        private string GenerateToken(CityInfoUser user)
        {
            var secKey = _configuration["Authentication:SecretKey"];
            //var encryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(secKey));
            var encryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
            var signingCreds = new SigningCredentials(encryptionKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("sub", user.UserId));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId));
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var iss = _configuration["Authentication:Issuer"];
            var aud = _configuration["Authentication:Audience"];

            var token = new JwtSecurityToken(iss, aud, claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(60), signingCreds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
