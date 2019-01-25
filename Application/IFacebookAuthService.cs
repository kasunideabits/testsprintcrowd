namespace SprintCrowd.Backend.Application
{
    using System.Threading.Tasks;

    /// <summary>
    /// Service interface for Facebook authentication.
    /// </summary>
    public interface IFacebookAuthService
    {
        /// <summary>
        /// Authenticates a given user using Facebook.
        /// A new user is created if the user does not exist.
        /// </summary>
        /// <param name="token">The Facebook access token.</param>
        /// <returns>A <see cref="Task{ExternalLoginResult}" /> represeting the invocation.</returns>
        Task<ExternalLoginResult> Authencticate(FacebookAuthInfo token);
    }
}