namespace SprintCrowd.Backend.Domain
{
    using System;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Represents a sprint crowd user.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets of sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName {get; set;}

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName {get; set;}
    }
}