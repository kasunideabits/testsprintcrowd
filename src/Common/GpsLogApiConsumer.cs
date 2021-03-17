using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Common
{
    public class GpsLogApiConsumer
    {
        static HttpClient client = new HttpClient();

        public GpsLogApiConsumer() { }

        private static string GetApiUrl()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return configuration.GetValue<string>("GpsLogApi:Url");
        }


        public async Task<int> GetTotalElevation(int sprintId, int userId)
        {
            string path = GetApiUrl() + "/elevation/getusertotalelevation"+ sprintId+"/"+ userId;

            var result = await this.ConsumeApi(path);

            return Convert.ToInt32(result.Data);
           
        }


        private async Task<ResponseObject> ConsumeApi(string path)
        {
            ResponseObject oResponseObject = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                oResponseObject = JsonConvert.DeserializeObject<ResponseObject>(jsonString);
            }
            return oResponseObject;
        }
    }
}
