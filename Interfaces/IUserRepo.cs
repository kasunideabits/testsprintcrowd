using SprintCrowdBackEnd.Models;

namespace SprintCrowdBackEnd.interfaces
{
    public interface IUserRepo
    {
        User GetUser(string email);
        void AddUser(User user);
        void UpdateUser(User user);
    }
}