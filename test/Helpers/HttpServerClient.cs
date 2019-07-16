namespace Tests.Helpers
{
  using System.IO;
  using System.Net.Http;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.TestHost;
  using Microsoft.Extensions.Configuration;
  public class HttpServerClient
  {
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional : false, reloadOnChange : true)
      .AddEnvironmentVariables()
      .Build();

    public static HttpClient CreateServerClient()
    {
      var builder = new WebHostBuilder().UseConfiguration(HttpServerClient.Configuration)
        .UseStartup<TestStartUp>();
      var testServer = new TestServer(builder);

      return testServer.CreateClient();
    }
  }
}