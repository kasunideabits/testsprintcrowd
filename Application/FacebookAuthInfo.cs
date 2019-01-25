using System.Collections.Generic;

namespace SprintCrowd.Backend.Application
{
    /// <summary>
    /// Represents a facebook access token.
    /// </summary>
    public class FacebookAuthInfo
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access FB access token.</value>
        public string AccessToken {get; set; }

        /// <summary>
        /// Gets or sets the email assicoated with the user.
        /// </summary>
        /// <value>The email.</value>
        /// <remarks>
        /// This is only used when registering a new user and
        /// the email address is not available in facebook or
        /// the token does not have the permissions to read it. 
        /// </remarks>
        public string Email { get; set; }

    }
}