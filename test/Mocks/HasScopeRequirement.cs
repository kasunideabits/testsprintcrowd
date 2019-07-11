using System;
using Microsoft.AspNetCore.Authorization;

namespace Tests.Mocks
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement()
        { }
    }
}