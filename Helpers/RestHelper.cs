

using System;
using Newtonsoft.Json;
using RestSharp;
using SprintCrowd.Backend.Enums;
using SprintCrowd.Backend.Logger;
using SprintCrowd.Backend.Models.GraphApi;

namespace SprintCrowd.Backend.Helpers
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
            SLogger.Log($"Sending request to {request.Resource}", LogType.Info);
            return JsonConvert.DeserializeObject<T>(
                _client.Execute(request).Content);
            
        }
    }
}