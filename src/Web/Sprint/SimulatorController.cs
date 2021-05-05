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

namespace SprintCrowdBackEnd.Web.Sprint
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorController : ControllerBase
    {
        private IUserService UserService { get; }

        private ISimulatorService SimulatorService { get; }

        public SimulatorController(
          ISimulatorService simulatorService)
        {
            this.SimulatorService = simulatorService;
        }

        [HttpPost("public/join")]

        public async Task<IActionResult> JoinEventPublic([FromBody] SprintSimulatorParticipants participants)
        {
            var result = await this.SimulatorService.JoinParticipants(participants.UsersCount, participants.SprintId);
            return this.Ok(result);

        }
    }
}
