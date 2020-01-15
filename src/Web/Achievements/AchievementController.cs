using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintCrowd.BackEnd.Common;
using SprintCrowd.BackEnd.Domain.Achievement;
using SprintCrowd.BackEnd.Domain.ScrowdUser;
using SprintCrowd.BackEnd.Extensions;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.Domain.Achievement;

namespace SprintCrowd.BackEnd.Web.Achievements
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AchievementController : ControllerBase
    {

        public AchievementController(IUserService userService, IAchievementService service)
        {
            this.AchievementService = service;
            this.UserService = userService;
        }

        private IAchievementService AchievementService { get; }
        private IUserService UserService { get; }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResponse<List<AchievementDto>>), 200)]
        public async Task<IActionResult> Get()
        {
            User user = await this.User.GetUser(this.UserService);
            var result = this.AchievementService.Get(user.Id);
            return this.Ok(new SuccessResponse<List<AchievementDto>>(result));
        }
    }
}