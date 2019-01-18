using SprintCrowdBackEnd.Helpers;
using SprintCrowdBackEnd.interfaces;
using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.services
{
    public class FbService: IFbService
    {
        private IFbRepo _fbRepo;

        public FbService(IFbRepo fbRepo)
        {
            this._fbRepo = fbRepo;
        }
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
        public Me GetFbUserDetails(string accessToken)
        {
            return _fbRepo.GetMe(accessToken);
        }
    }
}