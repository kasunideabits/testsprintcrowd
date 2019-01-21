using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.Interfaces
{
    public interface IFacebookReporsitory
    {
        FaceBoookUser GetUserProfile(string accessToken);
        DebugUserAccessToken DebugUserAccessToken(string accessToken);
    }
}