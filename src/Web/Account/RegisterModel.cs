namespace SprintCrowd.BackEnd.Web.Account
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// model for holding register data send to sprintcrowdbackend from mobile
    /// for registering user.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// access token
        /// TODO: for now only facebook access token, if future we may add login provider type
        /// to support google and other third party logins
        /// </summary>
        /// <value>facebook access token.</value>
        public string AccessToken { get; set; }
        /// <summary>
        /// gets or sets value
        /// </summary>
        /// <value>email of the user.</value>
        public string Email { get; set; }

        /// <summary>
        /// <see cref="LanguageCode"> Language preference for user </see>
        /// </summary>
        /// <value>language code</value>
        public string LanguagePreference { get; set; }
    }
}