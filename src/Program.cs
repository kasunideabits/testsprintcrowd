namespace SprintCrowd.BackEnd
{
    using System.IO;
    using System.Threading;
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RestSharp;
    using Serilog.Events;
    using Serilog.Sinks.SystemConsole.Themes;
    using Serilog;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Models;

    /// <summary>
    /// entry class for dotnet core application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// loading config from json
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional : false, reloadOnChange : true)
            .AddEnvironmentVariables()
            .Build();

        /// <summary>
        /// build webhost method, configuration etc, DO NOT MODIFY THIS,  Test project specially
        /// needs this method
        /// </summary>
        /// <param name="args">arguments</param>
        public static IWebHostBuilder CreateWebHostBuilder(string [] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(Program.Configuration)
            .UseSerilog((context, configuration) =>
            {
                configuration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme : AnsiConsoleTheme.Literate);
            })
            .UseUrls("http://0.0.0.0:7702")
            .UseStartup<Startup>();

        /// <summary>
        /// main method for dotnet core application
        /// </summary>
        public static void Main(string [] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            WaitForDependecyServices();

            IWebHost host = CreateWebHostBuilder(args).Build();

            using(IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                ScrowdDbContext context = provider.GetRequiredService<ScrowdDbContext>();
                context.Database.Migrate();
                context.SaveChanges();
            }

            host.Run();
        }

        /// <summary>
        /// waitis for dependecy services to come up
        /// </summary>
        private static void WaitForDependecyServices()
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            while (true)
            {
                var client = new RestClient(appSettings.AuthorizationServer);
                var request = new RestRequest(appSettings.OpenidConfigurationEndPoint, Method.GET);
                IRestResponse response = client.Get(request);
                if (response.IsSuccessful)
                {
                    Log.Logger.Information($"Identity server found");
                    break;
                }

                Log.Logger.Warning($"Identity server not up yet..  {appSettings.AuthorizationServer}/{appSettings.OpenidConfigurationEndPoint}");
                Thread.Sleep(3000);
            }
        }
    }
}
