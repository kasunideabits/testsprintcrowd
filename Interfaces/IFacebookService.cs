namespace SprintCrowdBackEnd.Interfaces
{
    using SprintCrowdBackEnd.Models.GraphApi;

    public interface IFacebookService
    {
        bool ValidateAccessToken(string accessToken);

        FaceBoookUser GetFbUserDetails(string accessToken);
    }
}