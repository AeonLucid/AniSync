using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}