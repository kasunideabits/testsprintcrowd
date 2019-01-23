namespace SprintCrowd.Backend.Interfaces
{
    using SprintCrowd.Backend.Models.GraphApi;

    public interface IFacebookService
    {
        bool ValidateAccessToken(string accessToken);

        FaceBookUser GetFbUserDetails(string accessToken);
    }
}