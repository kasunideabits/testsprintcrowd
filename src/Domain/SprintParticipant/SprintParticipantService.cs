namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
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
        /// <param name="sprintParticipantRepo">sprint participant repository</param>
        /// <param name="markAttendance">make attendace background notificaiton service</param>
        /// <param name="exitEventHandler">exit event background notificaiton service</param>
        public SprintParticipantService(
            ISprintParticipantRepo sprintParticipantRepo,
            IMarkAttendanceHandler markAttendance,
            IExitEventHandler exitEventHandler)
        {
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.MarkAttendance = markAttendance;
            this.ExitEventHandler = exitEventHandler;
        }

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
        /// Join user for a sprint
        /// </summary>
        /// <param name="sprintId">sprint id going to join</param>
        /// <param name="userId">user id who going to join</param>
        // TODO : notification
        public async Task JoinSprint(int sprintId, int userId)
        {
            try
            {
                await this.SprintParticipantRepo.AddSprintParticipant(sprintId, userId);
                this.SprintParticipantRepo.SaveChanges();
                return;
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

        /// <summary>
        /// Get all pariticipant with given stage <see cref="ParticipantStage"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="ParticipantInfo"> list of participant info</see></returns>
        public async Task<List<ParticipantInfo>> GetParticipants(int sprintId, ParticipantStage stage)
        {
            var joinedParticipants = await this.SprintParticipantRepo.GetParticipants(sprintId, stage);
            List<ParticipantInfo> participantInfos = new List<ParticipantInfo>();
            joinedParticipants.ForEach(p =>
            {
                var participant = new ParticipantInfo(
                    p.User.Id,
                    p.User.Name,
                    p.User.ProfilePicture,
                    p.User.Code,
                    p.Sprint.Id,
                    p.Sprint.Name);
                participantInfos.Add(participant);
            });
            return participantInfos;
        }

        /// <summary>
        /// Get sprint details with who marked attendance with given user id
        /// </summary>
        /// <param name="userId">user id to get record</param>
        /// <returns><see cref="SprintInfo">class </see></returns>
        public async Task<SprintInfo> GetSprintWhichMarkedAttendance(int userId)
        {
            Expression<Func<SprintParticipant, bool>> query = s =>
                s.UserId == userId &&
                s.Stage == ParticipantStage.MARKED_ATTENDENCE &&
                s.Sprint.Status == (int)SprintStatus.INPROGRESS;
            var markedAttendaceDetails = await this.SprintParticipantRepo.Get(query);
            if (markedAttendaceDetails != null)
            {
                return new SprintInfo(
                    markedAttendaceDetails.Sprint.Id,
                    markedAttendaceDetails.Sprint.Name,
                    markedAttendaceDetails.Sprint.Distance,
                    markedAttendaceDetails.Sprint.StartDateTime);
            }
            else
            {
                throw new Application.ApplicationException("NOT_FOUND_MARKED_ATTENDACE");
            }
        }
    }
}