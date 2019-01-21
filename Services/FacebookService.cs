

namespace SprintCrowdBackEnd.services
{
    using SprintCrowdBackEnd.Helpers;
    using SprintCrowdBackEnd.Interfaces;
    using SprintCrowdBackEnd.Models.GraphApi;

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
        public FaceBoookUser GetFbUserDetails(string accessToken)
        {
            return this._fbRepo.GetUserProfile(accessToken);
        }
    }
}