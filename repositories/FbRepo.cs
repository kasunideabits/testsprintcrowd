using RestSharp;
using SprintCrowdBackEnd.Helpers;
using SprintCrowdBackEnd.interfaces;
using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.repositories
{
    public class FbRepo: IFbRepo
    {
        private RestHelper _client;

        public FbRepo()
        {
            this._client = new RestHelper("https://graph.facebook.com/v3.2");
        }
        public Me GetMe(string accessToken)
        {
            RestRequest request = new RestRequest("me", Method.GET);
            request.AddParameter("access_token", accessToken);
            request.AddParameter("fields", "email,first_name,last_name");
            Me myDetails = _client.Execute<Me>(request);
            if(myDetails.Id == null)
            {
                return null;
            }
            return myDetails;
        }
    }
}