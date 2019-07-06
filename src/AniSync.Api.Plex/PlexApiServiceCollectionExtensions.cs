using AniSync.Data.Repositories;
using Flurl.Http;
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

                return client
                    .WithHeader("Accept", "application/json")
                    .WithHeader("X-Plex-Client-Identifier", config.GetPlexClientId())
                    .WithHeader("X-Plex-Product", "AniSync")
                    .WithHeader("X-Plex-Version", "3")
                    .WithHeader("X-Plex-Device", "AniSync")
                    .WithHeader("X-Plex-Platform", "Web");
            });
        }
    }
}
