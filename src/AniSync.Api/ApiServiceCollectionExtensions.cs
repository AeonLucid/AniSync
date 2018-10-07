using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AniSync.Api
{
    public static class ApiServiceCollectionExtensions
    {
        public static void AddApiClient<T>(this IServiceCollection services) where T : ApiClient
        {
            services.AddHttpClient(nameof(T));
            services.AddScoped(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(nameof(T));

                return (T) Activator.CreateInstance(typeof(T), httpClient);
            });
        }

        public static void AddApiClient<T>(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient) where T : ApiClient
        {
            services.AddHttpClient(nameof(T), configureClient);
            services.AddScoped(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(nameof(T));

                return (T) Activator.CreateInstance(typeof(T), httpClient);
            });
        }
    }
}
