
namespace SprintCrowd.Backend.Interfaces
{
    using SprintCrowd.Backend.Models;
    using SprintCrowd.Backend.Persistence;
    
    public interface IUserRepository
    {
        User GetUser(string email);

        void AddUser(User user);

        void UpdateUser(User user);
    }
}