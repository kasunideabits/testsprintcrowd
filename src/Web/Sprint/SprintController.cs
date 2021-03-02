namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
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
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="sprintService">sprint service</param>
        public SprintController(IUserService userService, ISprintService sprintService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
        }

        private ISprintService SprintService { get; }

        private IUserService UserService { get; }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="sprint">info about the sprint</param>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateSprintModel sprint)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSprint(
                user,
                sprint.Name,
                sprint.Distance,
                sprint.StartTime,
                sprint.SprintType,
                sprint.NumberOfParticipants,
                sprint.InfluencerEmail,
                sprint.DraftEvent,
                sprint.InfluencerAvailability,
                sprint.ImageUrl,
                sprint.promotionCode,
                sprint.IsTimeBased,
                sprint.DurationForTimeBasedEvent);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get sprint details with users who join to sprint
        /// </summary>
        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        [HttpGet("{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetSprintWithPaticipants(int sprintId)
        {
            var result = await this.SprintService.GetSprintWithPaticipants(sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get all Paticipants in sprint 
        /// </summary>
        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        [HttpGet("GetSprintPaticipants/{sprintId:int}/{pageNo:int}/{limit:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetSprintPaticipants(int sprintId, int pageNo, int limit)
        {
            var result = await this.SprintService.GetSprintPaticipants(sprintId, pageNo, limit);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }


        /// <summary>
        /// Get created sprint with userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="extendedTime">query param for extended time</param>
        /// <returns><see cref="">sprint pariticipants details with sprint</see></returns>
        [HttpGet("sprint-by-creator/{userId:int}")]
        [ProducesResponseType(typeof(SuccessResponse<SprintWithPariticpantsDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> GetSprintWithPaticipantsByCreator(int userId, int? extendedTime)
        {
            var result = await this.SprintService.GetSprintByCreator(userId, extendedTime);
            return this.Ok(new SuccessResponse<SprintWithPariticpantsDto>(result));
        }

        /// <summary>
        /// Remove sprint with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to remove</param>
        [HttpPost("remove/{sprintId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> RemoveSprint(int sprintId)
        {
            User user = await this.User.GetUser(this.UserService);
            await this.SprintService.Remove(user.Id, sprintId);
            return this.Ok();
        }

        /// <summary>
        /// update sprint
        /// </summary>
        [HttpPost("update/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateSprintModel sprint, int sprintId)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.UpdateSprint(
                user.Id,
                sprintId,
                sprint.Name,
                sprint.Distance,
                sprint.StartTime,
                sprint.NumberOfParticipants,
                sprint.InfluencerEmail,
                sprint.DraftEvent,
                sprint.ImageUrl,
                sprint.promotionCode,
                sprint.IsTimeBased,
                sprint.DurationForTimeBasedEvent);

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };

            return this.Ok(response);
        }

        /// <summary>
        /// Validate Sprint Edit By SprintId
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        [HttpGet("ValidateSprintEditBySprintId/{sprintId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> ValidateSprintEditBySprintId(int sprintId)
        {
            var result = await this.SprintService.ValidateSprintEditBySprintId(sprintId);
            return this.Ok(result);
        }

        /// <summary>
        /// Query public sprint with  utc offset
        /// </summary>
        /// <param name="timeOffset">time offset</param>
        /// <returns></returns>
        [HttpGet("public/start-now")]
        [ProducesResponseType(typeof(SuccessResponse<List<PublicSprintWithParticipantsDto>>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<dynamic> GetPublicSprintsWithPreference(int timeOffset)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.GetPublicSprints(user.Id, timeOffset);
            return this.Ok(new SuccessResponse<List<PublicSprintWithParticipantsDto>>(result));
        }

        /// <summary>
        /// Query public sprint with  utc offset
        /// </summary>
        /// <param name="timeOffset">time offset</param>
        /// <returns></returns>
        [HttpGet("public/open-events")]
        [ProducesResponseType(typeof(SuccessResponse<List<PublicSprintWithParticipantsDto>>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<dynamic> GetOpenEvents(int timeOffset)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.GetOpenEvents(user.Id, timeOffset);
            return this.Ok(new SuccessResponse<List<PublicSprintWithParticipantsDto>>(result));
        }


        /// <summary>
        /// Validate Private Sprint Count For User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("privatesprintcount/{userId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> ValidatePrivateSprintCountForUser(int userId)
        {
            int lapsTime = Common.PrivateSprint.PrivateSprintDefaultConfigration.LapsTime != null ? int.Parse(Common.PrivateSprint.PrivateSprintDefaultConfigration.LapsTime) : 15;
            int privateSprintCount = Common.PrivateSprint.PrivateSprintDefaultConfigration.PrivateSprintCount != null ? int.Parse(Common.PrivateSprint.PrivateSprintDefaultConfigration.PrivateSprintCount) : 100;
            var result = await this.SprintService.ValidatePrivateSprintCountForUser(userId, lapsTime, privateSprintCount);
            return this.Ok(result);
        }

        
    }
}