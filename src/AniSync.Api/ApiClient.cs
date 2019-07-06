using Flurl.Http;

namespace AniSync.Api
{
    public abstract class ApiClient
    {
        protected ApiClient(FlurlClient client)
        {
            Client = client;
        }

        protected FlurlClient Client { get; }
    }
}
