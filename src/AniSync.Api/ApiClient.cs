using System.Net.Http;
using Flurl.Http;

namespace AniSync.Api
{
    public abstract class ApiClient
    {
        protected ApiClient(HttpClient client)
        {
            Client = new FlurlClient(client);
        }

        protected FlurlClient Client { get; }
    }
}
