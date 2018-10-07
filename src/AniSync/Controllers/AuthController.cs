using AniSync.Api.Plex;
using AniSync.Data.Repositories;
using AniSync.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers
{
    public class AuthController : Controller
    {
        private readonly ConfigurationRepository _configuration;

        private readonly PlexApi _plexApi;

        public AuthController(ConfigurationRepository configuration, PlexApi plexApi)
        {
            _configuration = configuration;
            _plexApi = plexApi;
        }

        public IActionResult Login()
        {
            var requestUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            return View(new LoginViewModel(requestUrl, _configuration.GetPlexClientId()));
        }

        public IActionResult Redirect(
            [FromQuery(Name = "id")] string plexId,
            [FromQuery(Name = "code")] string plexCode)
        {
            return Redirect(_plexApi.GetOAuthUrl(plexId, plexCode, _configuration.GetPlexClientId()));
        }
    }
}