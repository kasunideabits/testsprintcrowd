namespace Tests
{
    using System.IO;
    using System.Net.Http;
    using System.Net;
    using System.Text;
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using Tests.Helpers;
    using Xunit;

    [Collection("Sequential")]
    public class SprintTests
    {
        private readonly HttpClient _client;

        public SprintTests()
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
            SprintModel newSprint = new SprintModel("Test", 1000, false, DateTime.UtcNow, (int)SprintType.PublicSprint, 0, 0, 1, 10);
            var response = await this._client.PostAsync("/sprint/create", new StringContent(
                JsonConvert.SerializeObject(newSprint, Formatting.None),
                Encoding.UTF8,
                "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Should update existing sprint
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async void ShouldUpdateSprint()
        {
            Sprint sprint = new Sprint();
            sprint.Distance = 1000;
            sprint.Name = "Test Sprint";
            sprint.LocationProvided = false;
            sprint.Lattitude = 0;
            sprint.Longitutude = 0;
            sprint.StartDateTime = DateTime.UtcNow;
            sprint.Type = (int)SprintType.PrivateSprint;
            var addedSprint = await TestStartUp.DbContext.Sprint.AddAsync(sprint);
            TestStartUp.DbContext.SaveChanges();
            SprintModel updateRequest = new SprintModel("Updated Sprint", 2000, false, DateTime.UtcNow, (int)SprintType.PublicSprint, 0, 0, addedSprint.Entity.Id, 10);

            var result = await this._client.PutAsync("/sprint/update", new StringContent(
                JsonConvert.SerializeObject(updateRequest, Formatting.None),
                Encoding.UTF8,
                "application/json"));
            dynamic responseObj = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
            Assert.Equal("Updated Sprint", (string)responseObj.Data.Name);
            Assert.Equal(2000, (int)responseObj.Data.Distance);
            Assert.Equal((int)SprintType.PublicSprint, (int)responseObj.Data.Type);
            Assert.Equal(10, (int)responseObj.Data.NumberOfParticipants);
            result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// should return all sprints
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async void GetAllSprints()
        {
            Sprint sprint1 = new Sprint();
            sprint1.Distance = 1000;
            sprint1.Name = "Test Sprint";
            sprint1.LocationProvided = false;
            sprint1.Lattitude = 0;
            sprint1.Longitutude = 0;
            sprint1.StartDateTime = DateTime.UtcNow;
            sprint1.Type = (int)SprintType.PublicSprint;
            Sprint sprint2 = new Sprint();
            sprint2.Distance = 1000;
            sprint2.Name = "Test Sprint";
            sprint2.LocationProvided = false;
            sprint2.Lattitude = 0;
            sprint2.Longitutude = 0;
            sprint2.StartDateTime = DateTime.UtcNow;
            sprint2.Type = (int)SprintType.PublicSprint;
            var addedSprint1 = await TestStartUp.DbContext.Sprint.AddAsync(sprint1);
            var addedSprint2 = await TestStartUp.DbContext.Sprint.AddAsync(sprint2);
            TestStartUp.DbContext.SaveChanges();

            var response = await this._client.GetAsync("/sprint/get-public");
            response.EnsureSuccessStatusCode();
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JsonConvert.DeserializeObject(strResponse);
            Assert.True((int)responseObj.Data.Count > 0 ? true : false);
        }

        /// <summary>
        /// Get live events
        /// </summary>
        [Fact]
        public async void GetLiveSprints()
        {
            Sprint sprint1 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 10000);
            Sprint sprint2 = GenerateSprint(DateTime.UtcNow.AddDays(1), SprintStatus.NOTSTARTEDYET, SprintType.PublicSprint, 10);
            Sprint sprint3 = GenerateSprint(DateTime.UtcNow.AddDays(-1), SprintStatus.ENDED, SprintType.PublicSprint, 10);
            Sprint sprint4 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 15000);
            Sprint sprint5 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 20000);
            Sprint sprint6 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 21000);
            Sprint sprint7 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 30000);

            await TestStartUp.DbContext.Sprint.AddAsync(sprint1);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint2);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint3);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint4);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint5);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint6);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint7);

            TestStartUp.DbContext.SaveChanges();

            var response = await this._client.GetAsync("/sprint/stat/live-events");
            response.EnsureSuccessStatusCode();
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JsonConvert.DeserializeObject(strResponse);
            Assert.Equal(5, (int)responseObj.Data.All);
            Assert.Equal(3, (int)responseObj.Data.TenToTwenty);
            Assert.Equal(2, (int)responseObj.Data.TwentyOneToThirty);
        }

        private Sprint GenerateSprint(
            DateTime startDateTime,
            SprintStatus status,
            SprintType type,
            int distance = 1000)
        {
            Random r = new Random();
            Sprint sprint = new Sprint()
            {
                Distance = distance,
                Name = "Test Sprint" + r.Next(10, 100).ToString(),
                LocationProvided = false,
                Lattitude = 0,
                Longitutude = 0,
                StartDateTime = startDateTime,
                Status = (int)status,
                Type = (int)type,
            };
            return sprint;
        }

    }
}