using System.IO;
using AniSync.Api.Plex;
using AniSync.Data.Extensions;
using AniSync.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AniSync
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Config = config;
        }

        private IConfiguration Config { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Initialise data directory.
            var dataDirectory = Config.GetValue<string>("DataDirectory");
            Directory.CreateDirectory(dataDirectory);

            // Register services.
            services.AddHttpContextAccessor();

            services.AddTransient<AuthService>();

            services.AddAniSyncDatabase(Path.Combine(dataDirectory, "AniSync.db"));

            services.AddPlexApi();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
