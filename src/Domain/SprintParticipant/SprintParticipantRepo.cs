﻿namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Implements ISprintParticipantRepo interface for hanle sprint participants
    /// </summary>
    public class SprintParticipantRepo : ISprintParticipantRepo
    {
        /// <summary>
        /// Initalize SprintParticipantRepo class
        /// </summary>
        /// <param name="context">dabase context</param>
        public SprintParticipantRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        /// <summary>
        /// Mark attendece for given sprint
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for mark attendance</param>
        /// <returns>User details</returns>
        public async Task<User> MarkAttendence(int sprintId, int userId)
        {
            SprintParticipant paritipant = await this.Context.SprintParticipant
                .FirstOrDefaultAsync(s => s.Sprint.Id == sprintId && s.User.Id == userId);
            if (paritipant == null)
            {
                throw new ApplicationException("Entry not found");

            }
            if (paritipant.Stage == (int)ParticipantStage.JOINED)
            {
                paritipant.Stage = (int)ParticipantStage.MARKED_ATTENDENCE;
                this.Context.SprintParticipant.Update(paritipant);
                return await this.Context.User.FirstOrDefaultAsync(u => u.Id == userId);
            }
            else if (paritipant.Stage == (int)ParticipantStage.MARKED_ATTENDENCE)
            {
                throw new ApplicationException("Already marked attendance");
            }
            else
            {
                throw new ApplicationException("Join before marked attendance");
            }
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="privateEventCreate">event model</param>
        /// <returns>added private sprint result</returns>
        public async Task<SprintParticipant> AddSprintParticipant(SprintParticipant privateEventCreate)
        {
            var result = await this.Context.SprintParticipant.AddAsync(privateEventCreate);
            return result.Entity;
        }

        /// <summary>
        /// Set participant stage to <see cref="ParticipantStage">QUIT</see>
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        public async Task<ParticipantInfo> ExitSprint(int sprintId, int userId)
        {
            var participant = await this.Context.SprintParticipant
                .Include(sp => sp.Sprint)
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.User.Id == userId && sp.Sprint.Id == sprintId);
            if (participant != null)
            {

                participant.Stage = (int)ParticipantStage.QUIT;
                this.Context.Update(participant);
                return new ParticipantInfo(
                    userId,
                    participant.User.Name,
                    participant.User.ProfilePicture,
                    sprintId,
                    participant.Sprint.Name);
            }
            else
            {
                throw new ApplicationException(ExitFaildReason.UserOrSprintNotMatch);
            }
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}