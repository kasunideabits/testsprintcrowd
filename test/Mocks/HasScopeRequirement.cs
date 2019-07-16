namespace Tests.Mocks
{
    using Microsoft.AspNetCore.Authorization;

    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement() { }
    }
}