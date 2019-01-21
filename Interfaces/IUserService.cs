using System.Collections.Generic;
using SprintCrowdBackEnd.Persistence;

namespace SprintCrowdBackEnd.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string fbAccessToken);
    }
}