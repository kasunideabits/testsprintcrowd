using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
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
        public static string GpsUrl {get;set;}

        public GpsLogApiConsumer() { }

        //private static string GetApiUrl()
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .AddEnvironmentVariables()
        //        .Build();
        //    return configuration.GetValue<string>("GpsLogApi:Url");
        //}


        public async Task<int> GetTotalElevation(int sprintId, int userId , string gpsApi)
        {
            string TODO = gpsApi;
            string path = "https://gpsapi-qa.sprintcrowd.com/elevation/getusertotalelevation/" + sprintId+"/"+ userId;

            Log.Logger.Information($"GpsApi path - {path}");
            var result = await this.ConsumeApi(path);

            if(result == null)
            Log.Logger.Information($" GetTotalElevation result NULL");

            return result != null ? Convert.ToInt32(result.Data) : 0;
           
        }


        private async Task<ResponseObject> ConsumeApi(string path)
        {
            Log.Logger.Information($" ConsumeApi path - {path}");
            ResponseObject oResponseObject = null;
            HttpResponseMessage response = await client.GetAsync(path);

            Log.Logger.Information($" ConsumeApi response - {response}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                oResponseObject = JsonConvert.DeserializeObject<ResponseObject>(jsonString);

                Log.Logger.Information($" ConsumeApi oResponseObject - {oResponseObject}");
            }
            return oResponseObject;
        }
    }
}
