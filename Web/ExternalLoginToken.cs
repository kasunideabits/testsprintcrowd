namespace SprintCrowd.Backend.Web
{
    /// <summary>
    /// Represents a token issued by an external login provider.
    /// </summary>
    public class ExternalLoginToken
    {
        /// <summary>
        /// Gets or sets the login provider that issued the token.
        /// </summary>
        /// <value>The login provider.</value>
        public ExternalLoginProvider LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the login token.
        /// </summary>
        /// <value>The login token.</value>
        public string Token { get; set; }

    }

}