namespace SprintCrowd.Backend.Web
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowdBackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Extensions;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// User authentication controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        IUserService userService;
        public TestController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<User> Index()
        {
            return await User.GetUser(this.userService);
        }
    }
}
