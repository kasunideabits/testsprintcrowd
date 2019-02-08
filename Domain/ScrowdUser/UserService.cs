using System.Threading.Tasks;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using SprintCrowdBackEnd.Web.Account;

namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        public UserService(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        /// <summary>
        /// Get facebook User
        /// </summary>
        /// <param name="userId">Facebook user id</param>
        public async Task<User> GetUser(string userId)
        {
            return await userRepo.GetUser(userId);
        }

        public async Task<User> RegisterUser(RegisterModel registerData)
        {
            User user =  await userRepo.RegisterUser(registerData);
            userRepo.SaveChanges();
            return user;
        }
    }
}