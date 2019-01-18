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
            request.AddParameter("format", "json");
            request.AddParameter("fields", "email,first_name,last_name");
            Me myDetails = _client.Execute<Me>(request);
            myDetails.ProfilePicture = GetProfilePictue(accessToken);
            if(myDetails.Id == null)
            {
                return null;
            }
            return myDetails;
        }
        private FbProfilePicture GetProfilePictue(string accessToken)
        {
           RestRequest request = new RestRequest("me/picture", Method.GET);
           request.AddParameter("access_token", accessToken);
           //should set redirect to false, other wise facebook replies with
           //base64 encoded image data
           request.AddParameter("redirect", false);
           request.AddParameter("format", "json");
           request.AddParameter("fields", "url,height,width");
           FbProfilePicture picture = _client.Execute<FbProfilePicture>(request);
           
           return picture;

        }
    }
}