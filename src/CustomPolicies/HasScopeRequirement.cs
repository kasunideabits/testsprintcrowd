namespace SprintCrowd.BackEnd.CustomPolicies
{
    using System;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// scope requirement
    /// </summary>
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// initializes an instance of HasScopeRequirement
        /// </summary>
        /// <param name="scope">scope of the user</param>
        /// <param name="issuer">issuer</param>
        public HasScopeRequirement(string scope, string issuer)
        {
            this.Scope = scope ??
                throw new ArgumentNullException(nameof(scope));
            this.Issuer = issuer ??
                throw new ArgumentNullException(nameof(issuer));
        }

        /// <summary>
        /// issuer
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// scope
        /// </summary>
        public string Scope { get; }
    }
}