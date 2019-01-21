namespace SprintCrowdBackEnd.Interfaces
{
    using SprintCrowdBackEnd.Models.GraphApi;

    public interface IFacebookService
    {
        bool ValidateAccessToken(string accessToken);

        FaceBookUser GetFbUserDetails(string accessToken);
    }
}