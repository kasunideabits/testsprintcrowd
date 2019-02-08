using System.Security.Claims;
using System.Threading.Tasks;
using SprintCrowdBackEnd.Domain.ScrowdUser;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowdBackEnd.Extensions
{
    public static class UserExtensions
    {
        public async static Task<User> GetUser(this ClaimsPrincipal claims, IUserService userService)
        {
            //Identity server maps sub to ClaimTypes.NameIdentifier
            return await userService.GetUser(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}