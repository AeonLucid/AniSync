using AniSync.Api.Plex;
using AniSync.Data.Models;
using AniSync.ViewModels.Auth;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers
{
    public class AuthController : Controller
    {
        private readonly PlexApi _plexApi;

        private readonly string _clientId;

        public AuthController(PlexApi plexApi, LiteDatabase database)
        {
            _plexApi = plexApi;
            _clientId = database.GetCollection<AniConfiguration>().FindOne(x => x.Key == AniConfigurationKey.PlexClientId).Value;
        }

        public IActionResult Login()
        {
            var requestUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            return View(new LoginViewModel(requestUrl, _clientId));
        }

        public IActionResult Redirect(
            [FromQuery(Name = "id")] string plexId,
            [FromQuery(Name = "code")] string plexCode)
        {
            return Redirect(_plexApi.GetOAuthUrl(plexId, plexCode, _clientId));
        }
    }
}