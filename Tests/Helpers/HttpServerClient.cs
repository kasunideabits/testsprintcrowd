using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Tests.Helpers
{
  public class HttpServerClient
  {
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    public static HttpClient CreateServerClient()
    {
      var builder = new WebHostBuilder().UseConfiguration(CreateSprintTest.Configuration)
      .UseStartup<TestStartUp>();
      var testServer = new TestServer(builder);

      return testServer.CreateClient();
    }
  }
}