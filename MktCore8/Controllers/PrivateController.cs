using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
namespace MonSite.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {

        public IActionResult Index()
        {

            return View();
        }

    }
}
