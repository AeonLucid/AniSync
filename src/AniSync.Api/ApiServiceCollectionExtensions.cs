using System;
using System.Net;
using System.Net.Http;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AniSync.Api
{
    public static class ApiServiceCollectionExtensions
    {
        public static void AddApiClient<T, TK>(this IServiceCollection services, 
            Func<IServiceProvider, FlurlClient, FlurlClient> configureFlurl,
            Func<IServiceProvider, TK> configureConfig) where T : ApiClient where TK : class
        {
            services.AddScoped(configureConfig);
            services.AddApiClient<T>(configureFlurl);
        }
        
        public static void AddApiClient<T>(this IServiceCollection services, Func<IServiceProvider, FlurlClient, FlurlClient> configureFlurl) where T : ApiClient
        {
            services.AddHttpClient(nameof(T)).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            services.AddScoped(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(nameof(T));
                var flurlClient = configureFlurl(provider, new FlurlClient(httpClient));

                return (T) Activator.CreateInstance(typeof(T), flurlClient);
            });
        }
    }
}
