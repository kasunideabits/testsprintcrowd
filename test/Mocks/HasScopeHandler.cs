namespace Tests.Mocks
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        /// <summary>
        /// authorization process
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}