namespace SprintCrowd.Backend.Infrastructure.ExternalLogin
{
    using System.Threading.Tasks;
    using RestSharp;

    public class FacebookProfileService : IFacebookProfileService
    {
        private const string FaceBookApiUrl = "https://graph.facebook.com/v3.2";

        private RestClient restClient;

        public FacebookProfileService()
        {
            this.restClient = new RestClient(FaceBookApiUrl);
        }
        
        public async Task<FacebookProfile> GetProfile(string accessToken)
        {
            RestRequest request = new RestRequest("me", Method.GET);
            request.AddParameter("access_token", accessToken);
            request.AddParameter("fields", "email,first_name,last_name, name");
            FacebookProfile profile = await this.restClient.GetAsync<FacebookProfile>(request);
            
            return profile;
        }
        
    }
}