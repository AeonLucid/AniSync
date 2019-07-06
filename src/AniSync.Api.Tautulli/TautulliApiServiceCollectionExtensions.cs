using AniSync.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AniSync.Api.Tautulli
{
    public static class TautulliApiServiceCollectionExtensions
    {
        public static void AddTautulliApi(this IServiceCollection services)
        {
            services.AddApiClient<TautulliApi>((provider, client) =>
            {
                var config = provider.GetRequiredService<ConfigurationRepository>();
client.
                return client.;
            });
        }
    }
}
