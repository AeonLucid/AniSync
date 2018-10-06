using System;
using AniSync.ViewModels.Auth;
using Flurl;
using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers
{
    public class AuthController : Controller
    {
        // TODO: Guid should be generated once and stored.
        private static readonly Guid guid = Guid.NewGuid();

        public IActionResult Login()
        {
            var requestUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            return View(new LoginViewModel(requestUrl, guid.ToString("N")));
        }

        public IActionResult Redirect(
            [FromQuery(Name = "id")] string plexId,
            [FromQuery(Name = "code")] string plexCode)
        {
            return Redirect("https://app.plex.tv/auth"
                .SetQueryParam("pinID", plexId)
                .SetQueryParam("code", plexCode)
                .SetQueryParam("context[device][product]", "AniSync")
                .SetQueryParam("context[device][environment]", "bundled")
                .SetQueryParam("context[device][layout]", "desktop")
                .SetQueryParam("context[device][platform]", "Web")
                .SetQueryParam("context[device][device]", "AniSync")
                .SetQueryParam("clientID", guid.ToString("N"))
                .ToString()
                .Replace("/auth", "/auth#"));
        }
    }
}