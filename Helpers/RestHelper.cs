

using System;
using Newtonsoft.Json;
using RestSharp;
using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.Helpers
{
    public class RestHelper
    {
        private RestClient _client;
        public RestHelper(string domain)
        {
            this._client = new RestClient(domain);
        }

        public IRestResponse Execute(RestRequest request)
        {
            return _client.Execute(request);
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            Console.WriteLine($"Sending request to {request.Resource}");
            string content = _client.Execute(request).Content;

            return JsonConvert.DeserializeObject<T>(content);
            /*return JsonConvert.DeserializeObject<T>(
                _client.Execute(request).Content);*/
            
        }
    }
}