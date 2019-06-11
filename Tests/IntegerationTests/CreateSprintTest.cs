namespace Tests
{
  using System;
  using System.IO;
  using System.Net;
  using System.Net.Http;
  using System.Text;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc.Testing;
  using Microsoft.AspNetCore.TestHost;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Newtonsoft.Json;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Domain.Sprint;
  using SprintCrowd.BackEnd.Infrastructure.Persistence;
  using Tests.Helpers;
  using Xunit;

  public class CreateSprintTest
  {
    private readonly HttpClient _client;

    public CreateSprintTest()
    {
      this._client = HttpServerClient.CreateServerClient();

    }

    /// <summary>
    /// should successfully create a sprint
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async void ShouldCreateNewSprint()
    {
      SprintModel newSprint = new SprintModel("Test", 1000, false, DateTime.UtcNow, (int)SprintType.PublicSprint, 0, 0);
      var response = await this._client.PostAsync("/sprint/create", new StringContent(
        JsonConvert.SerializeObject(newSprint, Formatting.None),
        Encoding.UTF8,
        "application/json"));

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}