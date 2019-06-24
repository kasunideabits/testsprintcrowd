using System;
using Microsoft.AspNetCore.Authorization;

namespace SprintCrowd.BackEnd.CustomPolicies
{
    /// <summary>
    /// scope requirement
    /// </summary>
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// issuer
        /// </summary>
        /// <value></value>
        public string Issuer { get; }
        /// <summary>
        /// scope
        /// </summary>
        /// <value></value>
        public string Scope { get; }

        /// <summary>
        /// initializes an instance of HasScopeRequirement
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="issuer"></param>
        public HasScopeRequirement(string scope, string issuer)
        {
            this.Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            this.Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}