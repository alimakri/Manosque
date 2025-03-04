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
        public IActionResult TokenUse()
        {
            MonLogger.LogInformation("HomeController.TokenUse");
            // Init
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token?.token);
            client.BaseAddress = new Uri(MaConfig["ApiServer:Url"]??"");
            MonLogger.LogInformation("HomeController.TokenUse {client.BaseAddress}", client.BaseAddress);

            // content
            StringContent content;
            try
            {
                content = new StringContent("{\"a\":1}", Encoding.UTF8, "application/json");
                HttpRequestMessage request = new(HttpMethod.Post, "api/comline") { Content = content };

                // Send
                Task<HttpResponseMessage> response = client.SendAsync(request); // <<<<<<<<<<<<<<<<<
                                                                                // Result
                if (response.Result != null)
                {
                    switch (response.Result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            ViewBag.Message = "Je suis passé !";
                            break;
                        default:
                            ViewBag.Message = $"Erreur d'un appel api : {response.Result.ReasonPhrase}";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur de connexion au service WebApi : {ex.Message}";
            }
            return View();
        }
        public IActionResult TokenDelete()
        {
            MonLogger.LogInformation("HomeController.TokenDelete");
            Token = null;
            return View();
        }
        public IActionResult TokenGet()
        {
            MonLogger.LogInformation("HomeController.TokenGet");
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(MaConfig["ApiServer:Url"]??"");
            Token = null;
            MonLogger.LogInformation("HomeController.TokenUse {client.BaseAddress}", client.BaseAddress);

            // content
            string json = ""; StringContent content;
            try
            {
                json = System.Text.Json.JsonSerializer.Serialize(new UserLogin { Username="ali", Password="password" });

                content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new(HttpMethod.Post, "api/comline/login") { Content = content };

                // Send
                Task<HttpResponseMessage> response = client.SendAsync(request); // <<<<<<<<<<<<<<<<<

                // Result
                if (response.Result != null)
                {
                    switch (response.Result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var jsonResult1 = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            Token = System.Text.Json.JsonSerializer.Deserialize<JwtToken>(jsonResult1);
                            ViewBag.Token = Token?.token;
                            break;
                        default:
                            ViewBag.Message = response.Result.ReasonPhrase;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur de connexion au service WebApi : {ex.Message}";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
