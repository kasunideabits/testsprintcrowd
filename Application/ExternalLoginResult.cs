namespace SprintCrowd.Backend.Application
{
    /// <summary>
    /// Represents the result of an external login operation.
    /// </summary>
    public class ExternalLoginResult
    {
        /// <summary>
        /// Gets or sets a value indicating if the user is aleady in the system.
        /// </summary>
        /// <value>Returns <c>true</c> if the user already exists, <c>false</c> if a new user is created.</value>
        public bool UserExists { get; }

        public string Token { get; }

        public ExternalLoginResult(bool userExists, string token)
        {
            this.UserExists = userExists;
            this.Token = token;
        }

    }
}