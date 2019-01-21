using SprintCrowdBackEnd.Models;
using SprintCrowdBackEnd.Persistence;

namespace SprintCrowdBackEnd.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string email);
        void AddUser(User user);
        void UpdateUser(User user);
    }
}