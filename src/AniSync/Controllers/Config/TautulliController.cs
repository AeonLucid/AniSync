using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers.Config
{
    [Route("Config/Tautulli")]
    public class TautulliController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Config/Tautulli/Index.cshtml");
        }
    }
}
