namespace SprintCrowdBackEnd.Interfaces
{
    using SprintCrowdBackEnd.Models.GraphApi;

    public interface IFacebookReporsitory
    {
        FaceBoookUser GetUserProfile(string accessToken);

        DebugUserAccessToken DebugUserAccessToken(string accessToken);
    }
}