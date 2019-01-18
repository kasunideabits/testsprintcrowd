using RestSharp;
using SprintCrowdBackEnd.Helpers;
using SprintCrowdBackEnd.interfaces;
using SprintCrowdBackEnd.Models.GraphApi;
using SprintCrowdBackEnd.Models;
using Microsoft.Extensions.Options;
using SprintCrowdBackEnd.Logger;
using SprintCrowdBackEnd.Enums;

namespace SprintCrowdBackEnd.repositories
{
    public class FbRepo: IFbRepo
    {
        private RestHelper _client;
        private AppSettings _appSettings;

        public FbRepo(IOptions<AppSettings> appSettings)
        {
            this._client = new RestHelper("https://graph.facebook.com/v3.2");
            this._appSettings = appSettings.Value;
        }

        private string GenerateAppAccessToken()
        {
            RestRequest request = new RestRequest("oauth/access_token", Method.GET);
            request.AddParameter("client_id", _appSettings.FacebookApp.AppId);
            request.AddParameter("client_secret", _appSettings.FacebookApp.AppSecret);
            request.AddParameter("grant_type", "client_credentials");
            OAuthAppAccessToken appAccessToken = _client.Execute<OAuthAppAccessToken>(request);

            return appAccessToken.AccessToken;
        }

        public DebugUserAccessToken DebugUserAccessToken(string accessToken)
        {
            RestRequest request = new RestRequest("debug_token", Method.GET);
            request.AddParameter("input_token", accessToken);
            request.AddParameter("access_token", GenerateAppAccessToken());

            return _client.Execute<DebugUserAccessToken>(request);
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