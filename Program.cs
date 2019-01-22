using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using SprintCrowdBackEnd.Enums;
using SprintCrowdBackEnd.Logger;

namespace SprintCrowdBackEnd
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public static void Main(string[] args)
        {
            IConfiguration sprintCrowdConfig = Configuration.GetSection("SprintCrowd");
            string hostUrl = sprintCrowdConfig.GetValue<string>("HostUrl");
            BuildWebHost(args, hostUrl).Build().Run();
        }

        public static IWebHostBuilder BuildWebHost(string[] args, string hostUrl) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(Program.Configuration)
                .UseUrls(hostUrl)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
