namespace Tests
{
    using System.Net.Http;
    using System.Net;
    using System.Text;
    using System;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Web.Event;
    using Tests.Helpers;
    using Xunit;

    [Collection("Sequential")]

    public class PrivateSprintTest
    {
        private readonly HttpClient _httpClient;

        public PrivateSprintTest()
        {
            this._httpClient = HttpServerClient.CreateServerClient();
        }

        /// <summary>
        /// should successfully create a private sprint
        /// </summary>
        [Fact]
        public async void ShouldCreateNewPrivateSprint()
        {
            SprintModel sprintModel = new SprintModel("TestEvent1", 1500, false, DateTime.UtcNow, (int)SprintType.PrivateSprint, 0, 0, 1, 3);
            var response = await this._httpClient.PostAsync("/privatesprint/create", new StringContent(JsonConvert.SerializeObject(sprintModel), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// should successfully join a private sprint
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async void ShouldJoinNewPrivateSprint()
        {
            JoinPrivateSprintModel sprintModel = new JoinPrivateSprintModel()
            {
                SprintId = 4,
                IsConfirmed = true
            };

            var response = await this._httpClient.PostAsync("/privatesprint/join", new StringContent(JsonConvert.SerializeObject(sprintModel), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}