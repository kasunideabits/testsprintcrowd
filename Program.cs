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
        public static void Main(string[] args)
        {
            // use this to allow command line parameters in the config
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            // use this to allow command line parameters in the config
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();


            /*var hostUrl = configuration["hosturl"];
            if (string.IsNullOrEmpty(hostUrl))
                hostUrl = "http://0.0.0.0:5000";*/

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseUrls(hostUrl)
                .UseSerilog()
                .Build();
        }
            
    }
}
