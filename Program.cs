namespace SprintCrowd.Backend
{
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            IConfiguration sprintCrowdConfig = Configuration.GetSection("SprintCrowd");
            string hostUrl = sprintCrowdConfig.GetValue<string>("HostUrl");
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(Program.Configuration)
                .UseUrls(hostUrl)
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();
        }

    }
}
