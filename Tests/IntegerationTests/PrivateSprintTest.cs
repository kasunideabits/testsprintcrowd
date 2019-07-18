namespace Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
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
        /// <returns></returns>
        [Fact]
        public async void ShouldCreateNewPrivateSprint()
        {
            SprintModel sprintModel = new SprintModel("TestEvent1", 1500, false, DateTime.UtcNow, (int)SprintType.PrivateSprint, 0, 0, 1, 3);
            var response = await this._httpClient.PostAsync("/privatesprint/private/create", new StringContent(JsonConvert.SerializeObject(sprintModel), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        /// <summary>
        /// should successfully join a private sprint
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async void ShouldJoinNewPrivateSprint()
        {
            // PrivateSprintModel sprintModel = new PrivateSprintModel(4, true);
            PrivateSprintModel sprintModel = new PrivateSprintModel()
            {
                SprintId = 4,
                IsConfirmed = true
            };

            var response = await this._httpClient.PostAsync("/privatesprint/private/join", new StringContent(JsonConvert.SerializeObject(sprintModel), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}