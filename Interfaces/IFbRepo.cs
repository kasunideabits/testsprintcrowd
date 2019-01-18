using SprintCrowdBackEnd.Models.GraphApi;

namespace SprintCrowdBackEnd.interfaces
{
    public interface IFbRepo
    {
        Me GetMe(string accessToken);
    }
}