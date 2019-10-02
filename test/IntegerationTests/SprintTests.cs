namespace Tests
{
    using System.Net.Http;
    using System.Net;
    using System.Text;
    using System;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
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
            SprintModel newSprint = new SprintModel("Test", 1000, "Colombo", DateTime.UtcNow, (int)SprintType.PublicSprint, 0, 0, 1, 10, true, "chamindi099@gmail.com");
            var response = await this._client.PostAsync("/sprintadmin/create", new StringContent(
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
            sprint.StartDateTime = DateTime.UtcNow;
            sprint.Type = (int)SprintType.PrivateSprint;
            var addedSprint = await TestStartUp.DbContext.Sprint.AddAsync(sprint);
            TestStartUp.DbContext.SaveChanges();
            SprintModel updateRequest = new SprintModel("Updated Sprint", 2000, "Colombo", DateTime.UtcNow, (int)SprintType.PublicSprint, 0, 0, addedSprint.Entity.Id, 10, true, "chamindi099@gmail.com");

            var result = await this._client.PutAsync("/sprintadmin/update", new StringContent(
                JsonConvert.SerializeObject(updateRequest, Formatting.None),
                Encoding.UTF8,
                "application/json"));
            dynamic responseObj = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
            Assert.Equal("Updated Sprint", (string)responseObj.Data.Name);
            Assert.Equal(2000, (int)responseObj.Data.Distance);
            Assert.Equal((int)SprintType.PublicSprint, (int)responseObj.Data.Type);
            //Assert.Equal(10, (int)responseObj.Data.NumberOfParticipants);
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
            sprint1.StartDateTime = DateTime.UtcNow;
            sprint1.Type = (int)SprintType.PublicSprint;
            Sprint sprint2 = new Sprint();
            sprint2.Distance = 1000;
            sprint2.Name = "Test Sprint";
            sprint2.StartDateTime = DateTime.UtcNow;
            sprint2.Type = (int)SprintType.PublicSprint;
            var addedSprint1 = await TestStartUp.DbContext.Sprint.AddAsync(sprint1);
            var addedSprint2 = await TestStartUp.DbContext.Sprint.AddAsync(sprint2);
            TestStartUp.DbContext.SaveChanges();

            var response = await this._client.GetAsync("/sprintadmin/get-public");
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
            Sprint sprint1 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 12000);
            Sprint sprint2 = GenerateSprint(DateTime.UtcNow.AddDays(1), SprintStatus.NOTSTARTEDYET, SprintType.PublicSprint, 10);
            Sprint sprint3 = GenerateSprint(DateTime.UtcNow.AddDays(-1), SprintStatus.ENDED, SprintType.PublicSprint, 10);
            Sprint sprint4 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 15000);
            Sprint sprint5 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 20000);
            Sprint sprint6 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 21000);
            Sprint sprint7 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 30000);
            Sprint sprint8 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 2000);
            Sprint sprint9 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 7000);

            await TestStartUp.DbContext.Sprint.AddAsync(sprint1);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint2);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint3);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint4);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint5);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint6);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint7);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint8);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint9);

            TestStartUp.DbContext.SaveChanges();

            var response = await this._client.GetAsync("/sprintadmin/stat/live-events");
            response.EnsureSuccessStatusCode();
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JsonConvert.DeserializeObject(strResponse);
            Assert.Equal(7, (int)responseObj.Data.All);
            Assert.Equal(2, (int)responseObj.Data.TwoToTen);
            Assert.Equal(3, (int)responseObj.Data.TenToTwenty);
            Assert.Equal(2, (int)responseObj.Data.TwentyOneToThirty);
        }

        /// <summary>
        /// Get created sprint for last week
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async void CreatedSprintCount()
        {
            Sprint sprint1 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 10000, DateTime.UtcNow.AddDays(-7));
            Sprint sprint2 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PrivateSprint, 10000, DateTime.UtcNow.AddDays(-7));
            Sprint sprint3 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 10000, DateTime.UtcNow.AddDays(-8));
            Sprint sprint4 = GenerateSprint(DateTime.UtcNow, SprintStatus.INPROGRESS, SprintType.PublicSprint, 10000, DateTime.UtcNow.AddDays(10));

            await TestStartUp.DbContext.Sprint.AddAsync(sprint1);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint2);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint3);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint4);

            TestStartUp.DbContext.SaveChanges();

            var to = DateTime.UtcNow.ToString();
            var from = DateTime.UtcNow.AddDays(-7).ToString();
            var response = await this._client.GetAsync($"/sprintadmin/stat/created-events?to={to}&from={from}");
            response.EnsureSuccessStatusCode();
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JsonConvert.DeserializeObject(strResponse);
            Assert.Equal(2, (int)responseObj.Data.Total);
            Assert.Equal(1, (int)responseObj.Data.Public);
            Assert.Equal(1, (int)responseObj.Data.Private);

        }

        private Sprint GenerateSprint(
            DateTime startDateTime,
            SprintStatus status,
            SprintType type,
            int distance = 1000,
            DateTime? createdDate = null)
        {
            Random r = new Random();
            Sprint sprint = new Sprint()
            {
                Distance = distance,
                Name = "Test Sprint" + r.Next(10, 100).ToString(),
                StartDateTime = startDateTime,
                Status = (int)status,
                Type = (int)type,
                CreatedDate = createdDate ?? DateTime.UtcNow
            };
            return sprint;
        }

    }
}