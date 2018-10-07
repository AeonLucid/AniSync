using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AniSync.Api.Plex;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace AniSync.Services.Auth
{
    public class AuthService
    {
        private readonly HttpContext _httpContext;

        private readonly PlexApi _plexApi;

        public AuthService(IHttpContextAccessor httpContextAccessor, PlexApi plexApi)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _plexApi = plexApi;
        }

        public async Task AuthenticateAsync(string authToken)
        {
            // Receive user data from Plex.
            var account = await _plexApi.GetAccountAsync(authToken);
                
            // Set response cookie.
            var claims = new List<Claim>
            {
                new Claim(AuthClaim.PlexId.ToString(), account.Id.ToString()),
                new Claim(AuthClaim.PlexToken.ToString(), account.AuthToken),
                new Claim(AuthClaim.PlexThumb.ToString(), account.Thumb.ToString()),
                new Claim(AuthClaim.Name.ToString(), account.Username),
                new Claim(AuthClaim.Email.ToString(), account.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                
            };
            
            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task SignOutAsync()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
