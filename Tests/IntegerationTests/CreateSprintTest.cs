using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Domain.Sprint;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using Tests.Helpers;
using Xunit;

namespace Tests
{
  public class CreateSprintTest : IClassFixture<WebApplicationFactory<SprintCrowd.BackEnd.Startup>>
  {
    private readonly WebApplicationFactory<SprintCrowd.BackEnd.Startup> _factory;
    private readonly HttpClient _client;
    private Auth _authHelper;

    public CreateSprintTest(WebApplicationFactory<SprintCrowd.BackEnd.Startup> factory)
    {
      this._authHelper = new Auth();
      _factory = factory.WithWebHostBuilder(config =>
      {
        config.ConfigureServices(services =>
        {
          services.AddDbContext<ScrowdDbContext>(options =>
                options.UseInMemoryDatabase("InMemory"));
        });
      });
      this._client = _factory.CreateClient();
    }

    /// <summary>
    /// should successfully create a sprint
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async void ShouldCreateNewSprint()
    {
      this._client.SetBearerToken(this._authHelper.GetAdminOuth2Token());
      SprintModel newSprint = new SprintModel("Test", 1000, false, DateTime.UtcNow, (int)SprintType.PublicSprint, 0, 0);
      var response = await this._client.PostAsync("/sprint/create", new StringContent(
        JsonConvert.SerializeObject(newSprint, Formatting.None),
        Encoding.UTF8,
        "application/json"));
      var b = response.Content.ReadAsStringAsync().Result;
      var a = "";
    }
  }
}