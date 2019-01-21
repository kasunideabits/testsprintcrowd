

namespace SprintCrowdBackEnd.Interfaces
{
    using System.Collections.Generic;
    using SprintCrowdBackEnd.Persistence;
    
    public interface IUserService
    {
        User Authenticate(string fbAccessToken);
    }
}