

namespace SprintCrowd.Backend.Interfaces
{
    using System.Collections.Generic;
    using SprintCrowd.Backend.Persistence;
    
    public interface IUserService
    {
        User Authenticate(string fbAccessToken);
    }
}