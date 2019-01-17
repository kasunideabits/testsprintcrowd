using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.interfaces
{
    public interface IFbService
    {
        bool ValidateAccessToken(string accessToken);
        Me GetFbUserDetails(string accessToken);
    }
}