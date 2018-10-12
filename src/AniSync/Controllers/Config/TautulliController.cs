using AniSync.Data.Repositories;
using AniSync.Data.Repositories.Config;
using AniSync.ViewModels.Config;
using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers.Config
{
    [Route("Config/Tautulli")]
    public class TautulliController : Controller
    {
        private readonly ConfigurationRepository _configuration;

        public TautulliController(ConfigurationRepository configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var config = _configuration.Get<ConfigCollectionTautulli>();

            return View("~/Views/Config/Tautulli/Index.cshtml", new TautulliViewModel
            {
                Enabled = config.Enabled,
                Endpoint = config.Endpoint,
                ApiKey = config.ApiKey
            });
        }

        [HttpPost("Save")]
        // [ValidateAntiForgeryToken]
        public IActionResult Save(TautulliViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _configuration.Save(new ConfigCollectionTautulli
            {
                Enabled = model.Enabled,
                Endpoint = model.Endpoint,
                ApiKey = model.ApiKey
            });

            return RedirectToAction("Index");
        }
    }
}
