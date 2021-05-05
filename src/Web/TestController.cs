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
    using System.Collections.Generic;
    
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
        // GET: api/Test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valueA1", "valueB2" };
        }
    }
}