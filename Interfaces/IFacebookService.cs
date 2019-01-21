using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.Interfaces
{
    public interface IFacebookService
    {
        bool ValidateAccessToken(string accessToken);
        FaceBoookUser GetFbUserDetails(string accessToken);
    }
}