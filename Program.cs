namespace SprintCrowd.Backend
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore;
    using Microsoft.Extensions.Configuration;
    using Serilog.Events;
    using Serilog.Sinks.SystemConsole.Themes;
    using Serilog;

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
        /// main method for dotnet core application
        /// </summary>
        public static void Main(string [] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// build webhost method, configuration etc
        /// </summary>
        /// <param name="args">arguments</param>
        public static IWebHost BuildWebHost(string [] args)
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
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme : AnsiConsoleTheme.Literate);
                })
                .UseUrls("http://0.0.0.0:5002")
                .UseStartup<Startup>()
                .Build();
        }

    }
}