using ComlineApp.Manager;
using ComlineServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace MktCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        ICoreComline MonComline;
        public ComlineController(ICoreComline comline)
        {
            MonComline = comline;
            if (!ServiceSystem.Options.ContainsKey("Service")) ServiceSystem.Options.Add("Service", "Data");
        }
        [HttpGet]
        public ActionResult Get()
        {
            return new ContentResult() { Content = "Comline Api Ok !" };
        }
        [HttpPost]
        public ActionResult Post([FromBody] Data data)
        {
            try
            {
                MonComline.Execute(new List<string?> { data.Command });

                string json = JsonConvert.SerializeObject(MonComline.Results);
                return new JsonResult(json)
                {
                    ContentType = "application/json",
                    StatusCode = 200 
                };

            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur s'est produite lors du traitement de la demande.");
            }
        }

    }
    public class Data
    {
        public string? Command { get; set; }
        public string? Script { get; set; }
    }
}
