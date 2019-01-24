using System.Collections.Generic;

namespace SprintCrowd.Backend.Web
{
    /// <summary>
    /// Represents a facebook access token.
    /// </summary>
    public class FacebookTokenInfo
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access FB access token.</value>
        public string AccessToken {get; set; }

        /// <summary>
        /// Gets or sets the permissions assiciated with the token.
        /// </summary>
        /// <value>The permissions.</value>
        public List<string> Permissions{ get; set; }

    }
}