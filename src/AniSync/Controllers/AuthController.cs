using System;
using System.Threading.Tasks;
using AniSync.Api.Plex;
using AniSync.Data.Repositories;
using AniSync.Responses;
using AniSync.Responses.Auth;
using AniSync.Services.Auth;
using AniSync.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AniSync.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly ConfigurationRepository _configuration;

        private readonly PlexApi _plexApi;

        private readonly AuthService _authService;

        public AuthController(ConfigurationRepository configuration, PlexApi plexApi, AuthService authService)
        {
            _configuration = configuration;
            _plexApi = plexApi;
            _authService = authService;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            var requestUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            return View(new LoginViewModel(requestUrl, _configuration.GetPlexClientId()));
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();

            return Redirect(Url.Action("Login"));
        }

        [HttpGet("Redirect")]
        public IActionResult Redirect(
            [FromQuery(Name = "id")] string plexId,
            [FromQuery(Name = "code")] string plexCode)
        {
            return Redirect(_plexApi.GetOAuthUrl(plexId, plexCode, _configuration.GetPlexClientId()));
        }

        [HttpPost("CheckPin")]
        public async Task<IActionResult> CheckPin([FromForm(Name = "pinId")] int pinId)
        {
            var pin = await _plexApi.GetPinAsync(pinId);

            // Validation.
            if (pin.ExpiresAt < DateTimeOffset.UtcNow)
            {
                return Ok(new ApiResponse<CheckPinData>
                {
                    Errors = new[]
                    {
                        new ApiResponseError("The pin has expired, please try again.")
                    },
                    Data =  new CheckPinData
                    {
                        Cancel = true,
                        Success = false,
                        Location = null
                    }
                });
            }

            if (string.IsNullOrEmpty(pin.AuthToken))
            {
                return Ok(new ApiResponse<CheckPinData>
                {
                    Errors = new[]
                    {
                        new ApiResponseError("User has not signed in yet.")
                    },
                    Data = new CheckPinData
                    {
                        Cancel = false,
                        Success = false,
                        Location = null
                    }
                });
            }

            // Authenticate.
            await _authService.AuthenticateAsync(pin.AuthToken);

            return Ok(new ApiResponse<CheckPinData>
            {
                Data = new CheckPinData
                {
                    Cancel = true,
                    Success = true,
                    Location = Url.Action("Index", "Home")
                }
            });
        }
    }
}