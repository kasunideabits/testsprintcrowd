
namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;

    /// <summary>
    /// user sprint controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]

    public class UsersSprintController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="sprintParticipantService">sprint participant service</param>
        public UsersSprintController(ISprintParticipantService sprintParticipantService)
        {
            this.SprintParticipantService = sprintParticipantService;
        }

        private IUserService UserService { get; }
        private ISprintParticipantService SprintParticipantService { get; set; }


        /// <summary>
        /// get joined users of the given sprint
        /// </summary>
        /// <param name="sprint_id">Id of the sprint</param>
        /// <param name="sprint_type">Type of the sprint</param>
        /// <param name="offset">Retrieve results from mark</param>
        /// <param name="fetch">Retrieve this much amount of results</param>

        [HttpGet]
        [Route("getusers")]
        public async Task<ResponseObject> GetJoinedUsers(int sprint_id, int sprint_type, int offset, int fetch)
        {
            var result = await this.SprintParticipantService.GetJoinedUsers(sprint_type, sprint_id, offset, fetch);

            return new ResponseObject
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result
            };
        }

    }

}