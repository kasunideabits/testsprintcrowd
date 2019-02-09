namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Web.Account;

    /// <summary>
    /// interface for UserService.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// retrieve the user by user id.
        /// </summary>
        /// <param name="userId">user id of the user.</param>
        Task<User> GetUser(string userId);
        /// <summary>
        /// register a user.
        /// </summary>
        /// <param name="registerData">register data of the user you want to register.</param>
        Task<User> RegisterUser(RegisterModel registerData);
    }
}