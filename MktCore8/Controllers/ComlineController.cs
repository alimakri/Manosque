using ComlineApp.Manager;
using ComLineCommon;
using ComLineData;
using ComlineServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MktCore8.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComlineController : ControllerBase
    {
        private IWebHostEnvironment Env;
        ICoreComline MonComline;
        public ComlineController(ICoreComline comline, IWebHostEnvironment env)
        {
            Env = env;
            MonComline = comline;
            Global.WorkingDirectory_ServiceData = Global.WorkingDirectory_ServiceSystem = 
                $@"{Env.WebRootPath.Replace("wwwroot", "documents")}\";
            if (!ServiceSystem.Options.ContainsKey("Service")) ServiceSystem.Options.Add("Service", "System"); else ServiceSystem.Options["Service"] = "System";
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

            var comline = ((CoreComline)MonComline);
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
                        comline.Command.Prompts = data.Script.Split(';').ToList();
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

    }
    public class Data
    {
        public string? Command { get; set; }
        public string? Script { get; set; }
    }
}
