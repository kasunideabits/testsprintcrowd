namespace SprintCrowdBackEnd.Extensions
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using SprintCrowdBackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  User extensions.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// get current authenticatede user.
        /// </summary>
        /// <param name="claims">extension for ClaimsPrincipal.</param>
        /// <param name="userService">users are retrieved using this service.</param>
        public async static Task<User> GetUser(this ClaimsPrincipal claims, IUserService userService)
        {
            // Identity server maps sub to ClaimTypes.NameIdentifier
            return await userService.GetFacebookUser(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}