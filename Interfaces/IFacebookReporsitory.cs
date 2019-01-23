namespace SprintCrowd.Backend.Interfaces
{
    using SprintCrowd.Backend.Models.GraphApi;

    public interface IFacebookReporsitory
    {
        FaceBookUser GetUserProfile(string accessToken);

        DebugUserAccessToken DebugUserAccessToken(string accessToken);
    }
}