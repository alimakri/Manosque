using ComLineCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MktCore8.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration config, IUserService userService) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly IUserService _userService = userService;

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenString = GenerateJWT(user.Username);
            return Ok(new { Token = tokenString });
        }

        private string GenerateJWT(string username)
        {
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
}
