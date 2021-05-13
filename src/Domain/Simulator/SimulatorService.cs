using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Domain.ScrowdUser;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Simulator
{
    public class SimulatorService : ISimulatorService
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
                    await this.sprintParticipantRepo.JoinSprint(user.Id, sprintId);
                }
            }
            catch (Exception ex)
            {                
                return false;
            }

            return true;
        }


        public async Task<List<ParticipantInfoDto>> GetParticipants(int sprintId, ParticipantStage stage)
        {
            var joinedParticipants = await this.sprintParticipantRepo.GetParticipants(sprintId, stage);
            List<ParticipantInfoDto> participantInfos = new List<ParticipantInfoDto>();
            joinedParticipants.ForEach(p =>
            {
                var participant = new ParticipantInfoDto(
                    p.User.Id,
                    p.User.Name,
                    p.User.ProfilePicture,
                    p.User.Code,
                    p.User.ColorCode,
                    p.User.City,
                    p.User.Country,
                    p.User.CountryCode,
                    p.Stage,
                    false
                    );
                participantInfos.Add(participant);
            });
            return participantInfos;
        }

        
    }
}
