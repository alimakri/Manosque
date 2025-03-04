using ComlineApp.Manager;
using ComLineCommon;
using ComLineData;
using ComlineServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MktCore8.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICoreComline _comline;
        private readonly ILogger<ComlineController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public ComlineController(IConfiguration config, ICoreComline comline, IWebHostEnvironment env, ILogger<ComlineController> logger, IUserService userService)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
            _env = env;
            _comline = comline;

            Global.WorkingDirectory_ServiceData = Global.WorkingDirectory_ServiceSystem =
                $@"{_env.WebRootPath.Replace("wwwroot", "documents")}\";
            if (!ServiceSystem.Options.TryAdd("Service", "System")) ServiceSystem.Options["Service"] = "System";
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            return new ContentResult() { Content = "Comline Api Ok !" };
        }

        [HttpPost]
        public ActionResult Post([FromBody] Data data)
        {

            var comline = ((CoreComline)_comline);
            if (comline != null)
            {
                try
                {
                    if (data.Command != null)
                    {
                        comline.Command.Prompts = [data.Command];
                    }
                    else if (data.Script != null)
                    {
                        comline.Command.Prompts = [.. data.Script.Split(';', StringSplitOptions.RemoveEmptyEntries)];
                    }
                    while (comline.Command.Prompts.Count > 0)
                    {
                        comline.Command.Reset();
                        comline.Execute();
                        comline.Command.Prompts.RemoveAt(0);
                    }
                    ResultList ds = comline.Command.Results;
                    string json = JsonConvert.SerializeObject(ds, Formatting.None);
                    var result = new JsonResult(json) { ContentType = "application/json" };
                    return result;
                }
                catch (Exception)
                {
                }
            }
            return StatusCode(500, "Une erreur s'est produite lors du traitement de la demande.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            _logger.LogInformation("AuthController.Login");

            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null) return Unauthorized();


            var tokenString = GenerateJWT(user.Username);
            _logger.LogInformation("AuthController.Login: tokenString {tokenString}", tokenString);
            return Ok(new { UserId = user.Id, Token = tokenString });
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
    public class Data
    {
        public string? Command { get; set; }
        public string? Script { get; set; }
    }
}
