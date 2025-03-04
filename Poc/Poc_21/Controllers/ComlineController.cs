using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Poc_21.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        private readonly ILogger<HomeController> MonLogger;
        private readonly IConfiguration _config;
        public ComlineController(ILogger<HomeController> logger, IConfiguration config)
        {
            MonLogger = logger;
            _config = config;

            MonLogger.LogInformation("ComlineController.ComlineController");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            MonLogger.LogInformation("ComlineController.Get");
            return new ContentResult() { Content = "Comline Get -> Ok !" };
        }
        [HttpPost]
        public ActionResult Post([FromBody] UserLogin user)
        {
            MonLogger.LogInformation("ComlineController.Post");
            return new ContentResult() { Content = "Comline Post -> Ok !" };
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            MonLogger.LogInformation("ComlineController.Login");
            if (user.Username != "ali" || user.Password != "password") return Unauthorized();

            var tokenString = GenerateJWT(user.Username);
            return Ok(new { UserId = user.Username, Token = tokenString });
        }
        private string GenerateJWT(string username)
        {
            MonLogger.LogInformation("ComlineController.GenerateJWT");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class UserLogin
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
