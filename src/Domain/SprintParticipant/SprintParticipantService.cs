﻿namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Notification.ExitEvent;
    using SprintCrowd.BackEnd.Domain.Notification.MarkAttendance;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Event;

    /// <summary>
    /// Implements ISprintParticipantService interface for hanle sprint participants
    /// </summary>
    public class SprintParticipantService : ISprintParticipantService
    {
        /// <summary>
        /// Initalize SprintParticipantService class
        /// </summary>
        /// <param name="sprintRepo">sprint repository</param>
        /// <param name="sprintParticipantRepo">sprint participant repository</param>
        /// <param name="markAttendance">make attendace background notificaiton service</param>
        public SprintParticipantService(
            ISprintRepo sprintRepo,
            ISprintParticipantRepo sprintParticipantRepo,
            IMarkAttendanceHandler markAttendance,
            IExitEventHandler exitEventHandler)
        {
            this.SprintRepo = sprintRepo;
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.MarkAttendance = markAttendance;
            this.ExitEventHandler = exitEventHandler;
        }

        private ISprintRepo SprintRepo { get; }

        private ISprintParticipantRepo SprintParticipantRepo { get; }

        private IMarkAttendanceHandler MarkAttendance { get; }

        private IExitEventHandler ExitEventHandler { get; }

        /// <summary>
        /// Mark the attendece for the given sprint and notify with evnet
        /// EventName.MarkedAttenence with MarkAttendenceMessage message
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for for participant</param>
        public async Task MarkAttendence(int sprintId, int userId)
        {
            var result = await this.SprintParticipantRepo.MarkAttendence(sprintId, userId);
            var mA = new MarkAttendance(sprintId, userId, result.Name, result.ProfilePicture);
            await this.MarkAttendance.Execute(mA);
            this.SprintParticipantRepo.SaveChanges();
            return;
        }

        /// <summary>
        /// Mark the attendece for the given sprint and join
        /// </summary>
        /// <param name="privateSprintInfo">sprint id for mark attendance</param>
        /// <param name="joinedUserId">user id for for participant</param>
        public async Task<SprintParticipant> CreateSprintJoinee(JoinPrivateSprintModel privateSprintInfo, User joinedUserId)
        {
            try
            {
                Sprint currentSprint = await this.SprintRepo.GetSprint(privateSprintInfo.SprintId);
                SprintParticipant sprintToBeJoined = new SprintParticipant()
                {
                    User = joinedUserId,
                    Stage = (int)ParticipantStage.MARKED_ATTENDENCE,
                    Sprint = currentSprint,
                };

                SprintParticipant sprintParticipant = await this.SprintParticipantRepo.AddSprintParticipant(sprintToBeJoined);
                if (sprintParticipant != null)
                {
                    this.SprintParticipantRepo.SaveChanges();
                }
                return sprintParticipant;
            }
            catch (System.Exception ex)
            {
                throw new Application.ApplicationException($"{ex}");
            }
        }

        /// <summary>
        /// Exit sprint which join for event
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        /// <returns><see cref="ExitSprintResult"> Exist sprint result</see></returns>
        // TODO : notification
        public async Task<ExitSprintResult> ExitSprint(int sprintId, int userId)
        {
            try
            {
                ParticipantInfo participant = await this.SprintParticipantRepo.ExitSprint(sprintId, userId);
                this.SprintParticipantRepo.SaveChanges();
                var exitEvent = new ExitEvent(
                    participant.SprintId,
                    participant.SprintName,
                    participant.UserId,
                    participant.UserName,
                    participant.ProfilePicture);
                await this.ExitEventHandler.Execute(exitEvent);
                return new ExitSprintResult { Result = ExitResult.Success };
            }
            catch (Exception ex)
            {
                return new ExitSprintResult { Result = ExitResult.Faild, Reason = ex.Message.ToString() };
            }
        }
    }
}