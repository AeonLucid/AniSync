using System.Threading.Tasks;
using AniSync.Api.Plex.Responses;
using Flurl;
using Flurl.Http;

namespace AniSync.Api.Plex
{
    public class PlexApi : ApiClient
    {
        public PlexApi(FlurlClient client) : base(client)
        {
        }

        public string GetOAuthUrl(string pinId, string code, string clientId)
        {
            return "https://app.plex.tv/auth"
                .SetQueryParam("pinID", pinId)
                .SetQueryParam("code", code)
                .SetQueryParam("context[device][product]", "AniSync")
                .SetQueryParam("context[device][environment]", "bundled")
                .SetQueryParam("context[device][layout]", "desktop")
                .SetQueryParam("context[device][platform]", "Web")
                .SetQueryParam("context[device][device]", "AniSync")
                .SetQueryParam("clientID", clientId)
                .ToString()
                .Replace("/auth", "/auth#");
        }

        public async Task<PlexPinResponse> GetPinAsync(int pinId)
        {
            return await $"https://plex.tv/api/v2/pins/{pinId}"
                .WithClient(Client)
                .GetJsonAsync<PlexPinResponse>();
        }

        public async Task<PlexAccount> GetAccountAsync(string authToken)
        {
            var result = await "https://plex.tv/users/account.json"
                .WithHeader("X-Plex-Token", authToken)
                .GetJsonAsync<PlexAccountResponse>();

            if (!string.IsNullOrEmpty(result.Error))
            {
                throw new PlexApiException($"Error received when fetching Plex account: '{result.Error}'.");
            }

            if (result.User == null)
            {
                throw new PlexApiException("The response does not contain a Plex account.");
            }

            return result.User;
        }
    }
}
