using System.Net.Http;
using Flurl;

namespace AniSync.Api.Plex
{
    public class PlexApi : ApiClient
    {
        public PlexApi(HttpClient client) : base(client)
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
    }
}
