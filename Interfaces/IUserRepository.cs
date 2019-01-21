
namespace SprintCrowdBackEnd.Interfaces
{
    using SprintCrowdBackEnd.Models;
    using SprintCrowdBackEnd.Persistence;
    
    public interface IUserRepository
    {
        User GetUser(string email);

        void AddUser(User user);

        void UpdateUser(User user);
    }
}