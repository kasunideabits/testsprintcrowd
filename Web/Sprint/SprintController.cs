namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class SprintController : ControllerBase
    {
        private ISprintService SprintService;
        private IUserService UserService;
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="sprintService">sprint service</param>
        /// /// <param name="userService">user service</param>
        public SprintController(ISprintService sprintService, IUserService userService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
        }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>All public events available in database</returns>
        [HttpGet("get-public")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<ResponseObject> GetAllPublicEvents()
        {
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = await this.SprintService.GetAll((int)SprintType.PublicSprint)
            };
            return response;
        }

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>Toatal count of live events, 10-20KM and 21-30km</returns>
        [HttpGet("stat/live-events")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetLiveSprintCount()
        {
            LiveSprintCount liveSprintsCount = await this.SprintService.GetLiveSprintCount();
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = liveSprintsCount
            };
            return this.Ok(response);
        }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="sprintInfo">info about the sprint</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ResponseObject> CreateEvent([FromBody] SprintModel sprintInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSprint(sprintInfo, user);

            return new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result
            };
        }

        /// <summary>
        /// update sprint
        /// </summary>
        [HttpPut]
        [Route("update")]
        public async Task<ResponseObject> UpdateEvent([FromBody] SprintModel SprintData)
        {
            Sprint sprint = await this.SprintService.UpdateSprint(SprintData);

            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = sprint };
        }
    }
}