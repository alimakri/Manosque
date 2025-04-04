﻿using ComlineApp.Manager;
using ComLineCommon;
using ComLineData;
using ComlineServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using MktCore8.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Data;

namespace MktCore8.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HomeController> _log;
        private readonly IConfiguration _config;
        private readonly ICoreComline _comline;
        private readonly IUserService _userService;

        public ComlineController(ICoreComline comline, ILogger<HomeController> logger, IConfiguration config, IWebHostEnvironment env, IUserService userService)
        {
            _log = logger;
            _config = config;
            _env = env;
            _comline = comline;
            _userService = userService;

            _log.LogInformation("ComlineController.ComlineController");

            // Init WorkingDirectories
            Global.WorkingDirectory_ServiceData =
                Global.WorkingDirectory_ServiceSystem = $@"{_env.WebRootPath.Replace("wwwroot", "documents")}\";
            // Default service is System
            if (!ServiceSystem.Options.TryAdd("Service", "System")) ServiceSystem.Options["Service"] = "System";
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            _log.LogInformation("ComlineController.Get");
            return new ContentResult() { Content = "Comline Get -> Ok !" };
        }


        [HttpPost]
        public ActionResult Post([FromBody] Data data)
        {
            int count = 0;
            _log.LogInformation("ComlineController.Post");
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
                        count++;
                    }
                    comline.Command.Results.AddInfo($"Nombre de requête comline : {count}", "Info");
                    var ds = ConvertDataSetToDictionary(comline.Command.Results);
                    //string json = JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.None);
                    var result = new JsonResult(ds) { ContentType = "application/json" };
                    return result;
                }
                catch (Exception ex)
                {
                    comline.Command.Results.AddError(ex.Message, ErrorCodeEnum.InternalError);
                }
            }
            return StatusCode(500, "Une erreur s'est produite lors du traitement de la demande.");
        }
        static Dictionary<string, List<Dictionary<string, string>>> ConvertDataSetToDictionary(DataSet dataSet)
        {
            Dictionary<string, List<Dictionary<string, string>>> dico = [];

            foreach (DataTable table in dataSet.Tables)
            {
                List<Dictionary<string, string>> dicoTable = [];
                foreach (DataRow row in table.Rows)
                {
                    Dictionary<string, string> dicoRow = [];
                    foreach (DataColumn column in table.Columns)
                    {
                        dicoRow.Add(column.ColumnName, row[column].ToString() ?? "");
                    }
                    dicoTable.Add( dicoRow);
                }
                dico.Add(table.TableName, dicoTable);
            }
            return dico;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            _log.LogInformation("AuthController.Login");
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null) return Unauthorized();


            var tokenString = GenerateJWT(user.Username);
            _log.LogInformation("AuthController.Login: tokenString {tokenString}", tokenString);
            return Ok(new { UserId = user.Id, Token = tokenString });
        }


        private string GenerateJWT(string username)
        {
            _log.LogInformation("ComlineController.GenerateJWT");
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
    public class Data
    {
        public string? Command { get; set; }
        public string? Script { get; set; }
    }
}
