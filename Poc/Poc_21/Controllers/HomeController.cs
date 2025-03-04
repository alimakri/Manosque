using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poc_21.Models;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Poc_21.Controllers
{
    public class JwtToken
    {
        public string userId { get; set; } = default;
        public string token { get; set; } = "";
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> MonLogger;
        private readonly IConfiguration MaConfig;
        public static JwtToken? Token;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            MaConfig = config;
            MonLogger = logger;
            MonLogger.LogInformation("HomeController.HomeController");
        }

        public IActionResult Index()
        {
            MonLogger.LogInformation("HomeController.Index");
            MonLogger.LogInformation("Thierno");
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
