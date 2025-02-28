using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MktCore8.Models;
using System.Diagnostics;

namespace MktCore8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> MonLogger;
        private readonly IConfiguration MaConfig;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            MaConfig = config;
            MonLogger = logger;
        }

        public IActionResult Index()
        {
            string? s = "Erreur"; ViewBag.ConnectionString = "Erreur ConnectionString";
            try
            {
                s = MaConfig.GetConnectionString("DefaultConnection");
                if (s != null)
                {
                    SqlConnection cnx = new SqlConnection(s);
                    cnx.Open();
                    ViewBag.ConnectionString = "Connexion ouverte";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "select Reference from Personne";
                    cmd.CommandType = System.Data.CommandType.Text;
                    var rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        ViewBag.Reader = rd.GetString(0);
                    }
                    rd.Close();
                }
            }
            catch (Exception) { ViewBag.Reader = "Erreur Reader"; }
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
