using System.Collections.Generic;
using SprintCrowdBackEnd.Models;

namespace SprintCrowdBackEnd.interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        ResponseObject RegisterUser(User user);
    }
}