using ComlineApp.Manager;
using ComlineApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Formatting = Newtonsoft.Json.Formatting;

namespace ComlineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        public CoreComline MonComline;
        public ComlineController(CoreComline mycomline)
        {
            MonComline = mycomline;
            if (!ServiceSystem.Options.ContainsKey("Service")) ServiceSystem.Options.Add("Service", "Data");

        }

        [HttpPost]
        public ActionResult Post([FromBody] Data data)
        {
            try
            {
                MonComline.Execute(new List<string?> { data.Command });
                DataTable dt = MonComline.Results.Tables[0];
                string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var result = new JsonResult(json) { ContentType = "application/json" };
                Response.Headers.Append("X-Table-Name", dt.TableName);
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
