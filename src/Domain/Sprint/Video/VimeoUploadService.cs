namespace SprintCrowd.BackEnd.Domain.Sprint.Video
{
    using System.Threading.Tasks;
    using System;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using Serilog;
    public class VimeoUploadService : IVimeoUploadService
    {
        public async Task<object> GetVimeoUploadLink(int fileSize)
        {

            RestClient restClient = new RestClient("https://api.vimeo.com/me/videos");
            var vimeotoken = "cb57574511e0df3dc2188b4016d23604";

            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/vnd.vimeo.*+json;version=3.4");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "bearer " + vimeotoken);
            request.AddJsonBody(new
            {
                upload = new
                {
                    approach = "tus",
                    size = fileSize,
                },
            });

            Console.WriteLine("Before Get Video upload Link post call ");
            try
            {
                var response = restClient.Execute(request, Method.POST);
                dynamic data = JObject.Parse(response.Content);
                Console.WriteLine("Video upload Link after POST call");
                return data;
            }
            catch (Exception ex)
            {
                Log.Logger.Error($" Video upload Link - {ex}");
                return null;
            }

        }
    }
}