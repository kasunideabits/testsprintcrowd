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
        public SprintSimulatorController(IUserService userService, ISprintService sprintService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
        }

        private ISprintService SprintService { get; }
        private IUserService UserService { get; }

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


    }
}