namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
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
    public class PrivateSprintController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="sprintService">sprint service</param>
        /// /// <param name="userService">user service</param>
        public PrivateSprintController(ISprintService sprintService, IUserService userService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
        }

        private ISprintService SprintService { get; }
        private IUserService UserService { get; }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="modelInfo">info about the sprint</param>
        /// <returns></returns>

        [HttpPost]
        [Route("private/create")]
        public async Task<ResponseObject> CreateEvent([FromBody] SprintModel modelInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSprint(modelInfo, user);

            return new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result
            };
        }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="modelInfo">Id of the sprint</param>
        /// <returns></returns>

        [HttpPost]
        [Route("private/join")]
        //public async Task<ResponseObject> JoinEvent(int SprintId, bool IsConfirmed)
        public async Task<ResponseObject> JoinEvent([FromBody] PrivateSprintModel modelInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            if (modelInfo.IsConfirmed)
            {
                var result = await this.SprintService.CreateSprintJoinee(modelInfo, 1, user);

                return new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result
                };
            }
            else
            {
                return new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.BadRequest
                };
            }
        }

    }

}