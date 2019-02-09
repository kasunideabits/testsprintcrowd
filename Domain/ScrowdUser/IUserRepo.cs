namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Web.Account;

    /// <summary>
    /// interface for UserRepo.
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        /// returned the user by user id.
        /// </summary>
        /// <param name="userId">user id of the user.</param>
        Task<User> GetUser(string userId);
        /// <summary>
        /// register a user.
        /// </summary>
        /// <param name="registerData">registration data.</param>
        Task<User> RegisterUser(RegisterModel registerData);
        /// <summary>
        /// commit changes to db and save changes.
        /// </summary>
        void SaveChanges();
    }
}