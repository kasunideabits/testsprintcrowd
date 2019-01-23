namespace SprintCrowd.Backend.Web
{
    /// <summary>
    /// Represents the result of an external login operation.
    /// </summary>
    public class ExternalLoginResult
    {
        /// <summary>
        /// Gets or sets a value indicating if the user is aleady in the system.
        /// </summary>
        /// <value>Returns <c>true</c> if the user already exists, <c>false</c> otherwise.</value>
        public bool UserExists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the email address is required.
        /// </summary>
        /// <value>
        /// Returns <c>true</c> if the user should be prompted for email,
        /// <c>false</c> otherwise.
        /// </value>
        /// <remarks>This value is relevant only for new users.</remarks>
        public bool EmailRequred { get; set; }

    }
}