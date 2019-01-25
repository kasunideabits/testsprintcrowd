namespace SprintCrowd.Backend.Application
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SprintCrowd.Backend.Domain;
    using SprintCrowd.Backend.Infrastructure.ExternalLogin;

    /// <summary>
    /// Application service used to authenticate users via Facebook.
    /// </summary>
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly IFacebookProfileService facebookProfileService;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Initializes a new instance of <see cref="FacebookAuthService" />.
        /// </summary>
        /// <param name="facebookProfileService">The face book profile service.</param>
        /// <param name="userManager">The user manager.</param>
        public FacebookAuthService(
            IFacebookProfileService facebookProfileService,
            UserManager<ApplicationUser> userManager)
        {
            this.facebookProfileService = facebookProfileService;
            this.userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<ExternalLoginResult> Authencticate(FacebookAuthInfo token)
        {
            FacebookProfile profile = await this.facebookProfileService.GetProfile(token.AccessToken);
            ApplicationUser user = await this.userManager.FindByLoginAsync(LoginProviders.Facebook, profile.Id);
            if(user != null)
            {
                return new ExternalLoginResult(true, string.Empty);
            }
            else 
            {
                string email = token.Email;
                if(string.IsNullOrWhiteSpace(email))
                {
                    email = profile.Email;
                }
                if(!string.IsNullOrWhiteSpace(email))
                {
                    user = new ApplicationUser();
                    user.UserName = email;
                    user.Email = email;
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    IdentityResult userResult = await this.userManager.CreateAsync(user);
                    if(!userResult.Succeeded)
                    {
                        throw new ApplicationException("New user creation failed.");
                    }
                    UserLoginInfo facebookLogin = new UserLoginInfo(LoginProviders.Facebook, profile.Id, profile.Name);
                    IdentityResult facebookResult = await this.userManager.AddLoginAsync(user, facebookLogin);
                    if(!facebookResult.Succeeded)
                    {
                        throw new ApplicationException("Facebook login creation failed.");
                    }
                    return new ExternalLoginResult(false, string.Empty);
                }
                else
                {
                    throw new EmailNotProvidedException();
                }
            }
        }
    }
}