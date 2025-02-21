using ComlineApp.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MktCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ICoreComline MonComline;
        public ValuesController(ICoreComline comline)
        {
            MonComline = comline;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return new ContentResult() { Content = "Comline Api Ok !" };
        }
    }
}
