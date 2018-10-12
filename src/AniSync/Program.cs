using System;
using System.IO;
using AniSync.Config;
using AniSync.Data.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace AniSync
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Resolve data directory.
            var dataDirectory = Environment.GetEnvironmentVariable("ANISYNC_DATADIRECTORY") ??
                                Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Data");

            // Initialize data directory and check if succesful.
            Directory.CreateDirectory(dataDirectory);

            if (!Directory.Exists(dataDirectory))
            {
                Console.WriteLine($"Failed to initialize data directory at {dataDirectory}.");
                return 1;
            }

            // Create other directories we may need.
            Directory.CreateDirectory(Path.Combine(dataDirectory, "Logs"));

            // Configure logger.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(dataDirectory, "Logs", "AniSync.log"), 
                    rollingInterval: RollingInterval.Day, 
                    retainedFileCountLimit: 7)
                .CreateLogger();

            try
            {
                // Run application.
                CreateWebHostBuilder(args, dataDirectory).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, string dataDirectory) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                })
                .ConfigureServices(services =>
                {
                    services.AddAniSyncDatabase(Path.Combine(dataDirectory, "AniSync.db"));

                    services.Configure<AniSyncConfig>(config =>
                    {
                        config.DataDirectory = dataDirectory; 
                    });
                })
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
