using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Common;
using SprintCrowd.BackEnd.Domain.ScrowdUser;
using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
using SprintCrowd.BackEnd.Extensions;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Web.Event;
using SprintCrowd.BackEnd.Web.Sprint.Models;
using SprintCrowdBackEnd.Web.Sprint.Models;
using SprintCrowdBackEnd.Domain.Simulator;
using System.Threading;

namespace SprintCrowdBackEnd.Web.Sprint
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorController : ControllerBase
    {
        private IUserService UserService { get; }

        private ISimulatorService SimulatorService { get; }
        private ISprintParticipantService SprintParticipantService { get; }

        public SimulatorController(
          ISimulatorService simulatorService, ISprintParticipantService sprintParticipantService)
        {
            this.SimulatorService = simulatorService;
            this.SprintParticipantService = sprintParticipantService;
        }

        [HttpPost("public/join")]

        public async Task<IActionResult> JoinEventPublic([FromBody] SprintSimulatorParticipants participants)
        {
            var result = await this.SimulatorService.JoinParticipants(participants.UsersCount, participants.SprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };

            return this.Ok(response);

        }

        [HttpPost("mark-attendance")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> MarkAttendence([FromBody] int SprintId)
        {
            var participants = await this.SimulatorService.GetParticipants(SprintId, ParticipantStage.JOINED);

            int index = 0;
            foreach (var user in participants)
            {
                await this.SprintParticipantService.MarkAttendence(SprintId, user.Id);
                // Ably chanell supports for 50 messages per second.
                if (index % 40 == 0)
                {
                    Thread.Sleep(2000);
                }

                if (index == 25) { break; }
                index++;
            }

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = "Successfully update mark attendence",
            };
            return this.Ok(response);
        }
    }
}
