using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using Tests.Helpers;
using Xunit;

namespace Tests
{
    [Collection("Sequential")]
    public class AppDownloadTests
    {

        private readonly HttpClient _client;

        public AppDownloadTests()
        {
            this._client = HttpServerClient.CreateServerClient();
        }

        [Fact]
        public async void GetAppDownloadInfo()
        {
            AppDownloads appDownload = new AppDownloads()
            {
                DeviceId = "asdadasd",
                DevicePlatform = "ANDROID"
            };
            AppDownloads appDownload2 = new AppDownloads()
            {
                DeviceId = "asdadasdaa",
                DevicePlatform = "IOS"
            };
            var addedAppDownload = await TestStartUp.DbContext.AppDownloads.AddAsync(appDownload);
            var addedAppDownload2 = await TestStartUp.DbContext.AppDownloads.AddAsync(appDownload2);
            await TestStartUp.DbContext.SaveChangesAsync();
            var response = await this._client.GetAsync("/device/info?null");
            response.EnsureSuccessStatusCode();
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JsonConvert.DeserializeObject(strResponse);
            Assert.Equal((int)responseObj.Data.All, 2);
            Assert.Equal((int)responseObj.Data.IOS, 1);
            Assert.Equal((int)responseObj.Data.Android, 1);
        }

        [Fact]
        public async void SetAppDownloadInfo()
        {
            AppDownloads appDownload = new AppDownloads()
            {
                DeviceId = "asdadasd",
                DevicePlatform = "ANDROID"
            };
            var response = await this._client.PostAsync("/sprint/create", new StringContent(
              JsonConvert.SerializeObject(appDownload, Formatting.None),
              Encoding.UTF8,
              "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}