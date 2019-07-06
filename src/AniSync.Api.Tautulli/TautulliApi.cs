using System.Net.Http;
using Flurl.Http;

namespace AniSync.Api.Tautulli
{
    public class TautulliApi : ApiClient
    {
        public TautulliApi(FlurlClient client) : base(client)
        {
        }
    }
}
