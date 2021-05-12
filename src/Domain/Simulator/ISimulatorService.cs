using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Simulator
{
    public interface ISimulatorService
    {
        Task<bool> JoinParticipants(int userCount, int sprintId);
        Task<List<ParticipantInfoDto>> GetParticipants(int sprintId, ParticipantStage stage);
    }
}
