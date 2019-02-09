namespace SprintCrowd.Backend.Web
{
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowdBackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Extensions;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// just a test controller used for quick code testing.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        IUserService userService;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        public TestController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// just a testing endpoint used for quick code testing
        /// </summary>
        [HttpGet]
        public async Task<User> Index()
        {
            return await this.User.GetUser(this.userService);
        }
    }
}