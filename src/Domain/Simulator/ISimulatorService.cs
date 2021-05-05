using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Simulator
{
    public interface ISimulatorService
    {
        Task<bool> JoinParticipants(int userCount, int sprintId);
    }
}
