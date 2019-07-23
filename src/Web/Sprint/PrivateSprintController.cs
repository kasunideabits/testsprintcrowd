namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PrivateSprintController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="sprintService">sprint service</param>
        /// <param name="sprintParticipantService">sprint participant service</param>
        public PrivateSprintController(
            IUserService userService,
            ISprintService sprintService,
            ISprintParticipantService sprintParticipantService)
        {
            this.SprintService = sprintService;
            this.SprintParticipantService = sprintParticipantService;
            this.UserService = userService;
        }

        private ISprintService SprintService { get; }

        private ISprintParticipantService SprintParticipantService { get; }

        private IUserService UserService { get; }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="modelInfo">info about the sprint</param>
        [HttpPost]
        [Route("create")]
        public async Task<ResponseObject> CreateEvent([FromBody] SprintModel modelInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSprint(modelInfo, user);

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return response;
        }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="modelInfo">Id of the sprint</param>
        [HttpPost]
        [Route("join")]
        public async Task<ResponseObject> JoinEvent([FromBody] JoinPrivateSprintModel modelInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            if (modelInfo.IsConfirmed)
            {
                var result = await this.SprintParticipantService.CreateSprintJoinee(modelInfo, user);

                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result,
                };
                return response;
            }
            else
            {
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.BadRequest,
                };
                return response;
            }
        }
    }
}