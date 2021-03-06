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


        /// <summary>
        /// gets or sets value
        /// </summary>
        /// <value>Name of the user.</value>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets value
        /// </summary>
        /// <value>UserType of the user.</value>
        public int UserType { get; set; }
    }
    /// <summary>
    /// User register by Email class
    /// </summary>
    public class EmailUser
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
        public string VerificationCode { get; set; }
        public string PromotionCode { get; set; }
        public int SprintId { get; set; }
    }
}