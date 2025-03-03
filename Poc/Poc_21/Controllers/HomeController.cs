using Microsoft.AspNetCore.Mvc;
using Poc_21.Models;
using System.Diagnostics;

namespace Poc_21.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> MonLogger;
        private readonly IConfiguration MaConfig;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            MonLogger = logger;
            MonLogger.LogInformation("Va dormir...");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
