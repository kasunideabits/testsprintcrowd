namespace SprintCrowd.BackEnd.Web
{
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// just a test controller used for quick code testing.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    //   [Authorize]
    public class TestController : ControllerBase
    {
        IUserService userService;
        ISprintService sprintService;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        public TestController(IUserService userService, ISprintService sprintService)
        {
            this.userService = userService;
            this.sprintService = sprintService;
        }

        /// <summary>
        /// just a testing endpoint used for quick code testing
        /// </summary>
        [HttpGet]
        public async Task<dynamic> Index(TimeSpan timeOffset)
        {
            Console.WriteLine(timeOffset.Days);
            var a = await this.sprintService.GetPublicSprints(3, -530);
            return a;
            //   return await this.User.GetUser(this.userService);
        }
    }
}