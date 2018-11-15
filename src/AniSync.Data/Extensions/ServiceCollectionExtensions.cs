using AniSync.Data.Repositories;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AniSync.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAniSyncDatabase(this IServiceCollection services, string databasePath)
        {
            services.AddSingleton(provider =>
            {
                var env = provider.GetRequiredService<IHostingEnvironment>();
                var databaseMapper = new BsonMapper().WithAniSyncMappings();

                return new LiteDatabase(databasePath, databaseMapper).WithAniSyncMigrations(env.IsDevelopment());
            });

            services.AddSingleton<ConfigurationRepository>();
            services.AddSingleton<UserRepository>();
        }
    }
}
