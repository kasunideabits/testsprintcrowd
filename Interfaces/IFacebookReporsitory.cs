namespace SprintCrowdBackEnd.Interfaces
{
    using SprintCrowdBackEnd.Models.GraphApi;

    public interface IFacebookReporsitory
    {
        FaceBookUser GetUserProfile(string accessToken);

        DebugUserAccessToken DebugUserAccessToken(string accessToken);
    }
}