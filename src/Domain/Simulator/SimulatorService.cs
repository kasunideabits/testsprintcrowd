using SprintCrowd.BackEnd.Domain.ScrowdUser;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Simulator
{
    public class SimulatorService
    {
        private readonly IUserRepo userRepo;
        private readonly ISprintParticipantRepo sprintParticipantRepo;

        public SimulatorService(IUserRepo userRepo, ISprintParticipantRepo sprintParticipantRepo)
        {
            this.userRepo = userRepo;
            this.sprintParticipantRepo = sprintParticipantRepo;
        }


        public async Task<bool> JoinParticipants(int userCount, int sprintId)
        {
            try
            {
                var list = await this.userRepo.GetRandomUsers_ForSimulator(userCount);

                foreach (var user in list)
                {
                    await this.sprintParticipantRepo.AddParticipant(sprintId, user.Id);
                }
            }
            catch (Exception ex)
            {                
                return false;
            }

            return true;
        }
    }
}
