using ComlineApp.Manager;
using ComlineServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Formatting = Newtonsoft.Json.Formatting;

namespace Manosque.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        public CoreComline MonComline;
        public ComlineController(CoreComline mycomline)
        {
            MonComline = mycomline;
            ServiceSystem.Options.TryAdd("Service", "Data");
        }

        [HttpPost]
        public ActionResult Post([FromBody] Data data)
        {
            try
            {
                MonComline.Execute([data.Command]);
                ResultList ds = MonComline.Results;
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                var result = new JsonResult(json) { ContentType = "application/json" };
                return result;
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
