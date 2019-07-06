using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AniSync.Api.Plex;
using AniSync.Data.Models;
using AniSync.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace AniSync.Services.Auth
{
    public class AuthService
    {
        private readonly HttpContext _httpContext;

        private readonly UserRepository _userRepository;

        private readonly PlexApi _plexApi;

        public AuthService(IHttpContextAccessor httpContextAccessor, UserRepository userRepository, PlexApi plexApi)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
            _plexApi = plexApi;
        }

        public async Task AuthenticateAsync(string authToken)
        {
            // Receive user data from Plex.
            var account = await _plexApi.GetAccountAsync(authToken);
            
            // Add user to database.
            if (!_userRepository.Exists(account.Id))
            {
                // If there is no admin yet, make this user an admin.
                _userRepository.Create(new AniUser
                {
                    Id = account.Id,
                    Admin = !_userRepository.ContainsAdmin()
                });
            }

            // Set response cookie.
            var claims = new List<Claim>
            {
                new Claim(AuthClaim.PlexId.ToString(), account.Id.ToString()),
                new Claim(AuthClaim.PlexToken.ToString(), account.AuthToken),
                new Claim(AuthClaim.PlexThumb.ToString(), account.Thumb.ToString()),
                new Claim(AuthClaim.Name.ToString(), account.Username),
                new Claim(AuthClaim.Email.ToString(), account.Email)
            };

            if (_userRepository.IsAdmin(account.Id))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            
            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task SignOutAsync()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
