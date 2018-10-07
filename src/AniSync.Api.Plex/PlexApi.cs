using System.Net.Http;

namespace AniSync.Api.Plex
{
    public class PlexApi : ApiClient
    {
        public PlexApi(HttpClient client) : base(client)
        {
        }
    }
}
