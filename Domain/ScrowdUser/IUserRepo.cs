using System.Threading.Tasks;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using SprintCrowdBackEnd.Web.Account;

namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    public interface IUserRepo
    {
        Task<User> GetUser(string userId);
        Task<User> RegisterUser(RegisterModel registerData);
        void SaveChanges();
    }
}