namespace SprintCrowd.Backend
{
    using System.IO;
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog.Events;
    using Serilog.Sinks.SystemConsole.Themes;
    using Serilog;
    using SprintCrowdBackEnd.Infrastructure.Persistence;

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
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        /// <summary>
        /// main method for dotnet core application
        /// </summary>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            IWebHost host = BuildWebHost(args);
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                ScrowdDbContext context = provider.GetRequiredService<ScrowdDbContext>();
                context.Database.Migrate();
                context.SaveChanges();
            }
            host.Run();
        }

        /// <summary>
        /// build webhost method, configuration etc
        /// </summary>
        /// <param name="args">arguments</param>
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(Program.Configuration)
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                })
                .UseUrls("http://0.0.0.0:5002")
                .UseStartup<Startup>()
                .Build();
        }

    }
}