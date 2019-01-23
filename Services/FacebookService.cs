namespace SprintCrowd.Backend.Services
{
    using SprintCrowd.Backend.Helpers;
    using SprintCrowd.Backend.Interfaces;
    using SprintCrowd.Backend.Models.GraphApi;

    public class FacebookService: IFacebookService
    {

        public FacebookService(IFacebookReporsitory fbRepo)
        {
            this._fbRepo = fbRepo;
        }

        private IFacebookReporsitory _fbRepo;

        public bool ValidateAccessToken(string accessToken)
        {
            DebugUserAccessToken userAccessTokenData =  _fbRepo.DebugUserAccessToken(accessToken);
            if(userAccessTokenData.Data != null && userAccessTokenData.Data.IsValid)
            {
                return true;
            }

            return false;
        }

        /*
            Returns UserId and Email of the Facebook user the access
            token belongs to
         */
        public FaceBookUser GetFbUserDetails(string accessToken)
        {
            return this._fbRepo.GetUserProfile(accessToken);
        }
    }
}