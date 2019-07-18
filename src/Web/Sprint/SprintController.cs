namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Enums;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy.ADMIN)]

    public class SprintController : ControllerBase
    {
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

        private ISprintService SprintService { get; }

        private IUserService UserService { get; }

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
                Data = await this.SprintService.GetAll((int)SprintType.PublicSprint),
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
                Data = liveSprintsCount,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get all created sprints, query filter can be apply with form, to
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>All, Public, Private created sprint count for given date range </returns>
        [HttpGet("stat/created-events")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetCreatedEventsCount(DateTime from, DateTime? to)
        {
            CreatedSprintCount createdSprints = await this.SprintService.GetCreatedEventsCount(from, to);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = createdSprints,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="sprintInfo">info about the sprint</param>
        [HttpPost]
        [Route("create")]
        public async Task<ResponseObject> CreateEvent([FromBody] SprintModel sprintInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSprint(sprintInfo, user);

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return response;
        }

        /// <summary>
        /// update sprint
        /// </summary>
        [HttpPut]
        [Route("update")]
        public async Task<ResponseObject> UpdateEvent([FromBody] SprintModel sprintData)
        {
            Sprint sprint = await this.SprintService.UpdateSprint(sprintData);

            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = sprint };
        }
    }
}