using AniSync.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AniSync.Api.Plex
{
    public static class PlexApiServiceCollectionExtensions
    {
        public static void AddPlexApi(this IServiceCollection services)
        {
            services.AddApiClient<PlexApi>((provider, client) =>
            {
                var config = provider.GetRequiredService<ConfigurationRepository>();

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("X-Plex-Client-Identifier", config.GetPlexClientId());
                client.DefaultRequestHeaders.Add("X-Plex-Product", "AniSync");
                client.DefaultRequestHeaders.Add("X-Plex-Version", "3");
                client.DefaultRequestHeaders.Add("X-Plex-Device", "AniSync");
                client.DefaultRequestHeaders.Add("X-Plex-Platform", "Web");
            });
        }
    }
}
