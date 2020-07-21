namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Account;
    using SprintCrowd.BackEnd.Web.Event;

    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SprintSimulatorController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SprintSimulatorController
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="sprintService">sprint service</param>
        /// <param name="sprintParticipantService">instance reference for ISprintParticipantService</param>
        public SprintSimulatorController(IUserService userService, ISprintService sprintService, ISprintParticipantService sprintParticipantService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
            this.SprintParticipantService = sprintParticipantService;
        }

        private ISprintService SprintService { get; }
        private IUserService UserService { get; }
        private ISprintParticipantService SprintParticipantService { get; }

        /// <summary>
        /// creates an simulation
        /// </summary>
        /// <param name="sprint">info about the sprint</param>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateSprintModel sprint)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSimulation(
                user,
                sprint.Name,
                sprint.Distance,
                sprint.StartTime,
                sprint.SprintType,
                sprint.NumberOfParticipants,
                sprint.InfluencerEmail,
                sprint.DraftEvent,
                sprint.InfluencerAvailability);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get dashboard data
        /// </summary>
        /// <returns>Dashboard related data</returns>
        [HttpGet("getallsprints")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetAllSprints()
        {
            var sprintsList = await this.SprintService.GetAllSprints();
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = sprintsList,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// update sprint
        /// </summary>
        [HttpPut("update/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateSimulationModel sprint, int sprintId)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.UpdateSimulation(
                user.Id,
                sprintId,
                sprint.Name,
                sprint.Distance,
                sprint.StartTime,
                sprint.NumberOfParticipants,
                sprint.InfluencerEmail,
                sprint.DraftEvent,
                sprint.Status);

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };

            return this.Ok(response);
        }

        /// <summary>
        /// Get all participant who join with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to look up</param>
        [HttpGet("getparticipants/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetAllParticipants(int sprintId)
        {
            var result = await this.SprintParticipantService.GetAllParticipants(sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Remove simulation participant
        /// </summary>
        /// <param name="sprintId">sprint id</param>
        /// <param name="participantId">creator id</param>
        [HttpDelete("participant/{sprintId:int}/{participantId:int}/")]
        public async Task<IActionResult> RemoveParticipant(int sprintId, int participantId)
        {
            await this.SprintParticipantService.RemoveSimulationParticipant(sprintId, participantId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="keyword">search keyword</param>
        /// <param name="simulationId">simulation id</param>
        [HttpGet("getallusers/{keyword}/{simulationId:int}")]
        public async Task<IActionResult> GetAllUsers(string keyword, int simulationId)
        {
            var result = await this.UserService.GetAllUsers(keyword, simulationId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result
            };
            return this.Ok(response);
        }

        /// <summary>
        /// add participants to an event
        /// </summary>
        /// <param name="userIds">list of user ids</param>
        /// <param name="sprintId">simulation id</param>
        [HttpPost("joinsimulation/{sprintId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> JoinSimulation([FromBody] List<int> userIds, int sprintId)
        {
            await this.SprintParticipantService.JoinSimulation(userIds, sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = "Participants added to simulation successfully"
            };
            return this.Ok(response);
        }

        /// <summary>
        /// mark attendance participants to an event
        /// </summary>
        /// <param name="userIds">list of user ids</param>
        /// <param name="sprintId">simulation id</param>
        [HttpPost("markattendancesimulation/{sprintId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> MarkAttendanceSimulation([FromBody] List<int> userIds, int sprintId)
        {
            await this.SprintParticipantService.MarkAttendanceSimulation(userIds, sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = "Participants marked attendance successfully"
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Exit simulation
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userIds">user ids which leaving the event</param>
        [HttpPost("exitsimulation/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> ExitSimulation([FromBody] List<int> userIds, int sprintId)
        {
            ExitSprintResult result = await this.SprintParticipantService.ExitSimulation(userIds, sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Upload users
        /// </summary>
        /// <param name="uploadData">upload user data</param>
        [HttpPost("uploaduser")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> UploadUser([FromBody] List<RegisterModel> uploadData)
        {
            // string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
            string baseUrl = "http://localhost:7702"; // for dev environment. temp fix
            using (var client = new HttpClient())
            {
                Console.WriteLine(baseUrl);
                client.BaseAddress = new Uri(baseUrl);
                var firstItem = uploadData[0];
                if (firstItem.Email == string.Empty || firstItem.AccessToken == null)
                {
                    return this.BadRequest();
                }
                else
                {
                    foreach (var data in uploadData)
                    {
                        string payload = JsonConvert.SerializeObject(data);
                        var content = new StringContent(payload, Encoding.UTF8, "application/json");
                        await client.PostAsync("/account/register", content);
                    }
                }
            }
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = "Users uploaded successfully",
            };
            return this.Ok(response);
        }

    }
}