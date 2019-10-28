namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Implements ISprintParticipantService interface for hanle sprint participants
    /// </summary>
    public class SprintParticipantService : ISprintParticipantService
    {
        /// <summary>
        /// Initalize SprintParticipantService class
        /// </summary>
        /// <param name="sprintParticipantRepo">sprint participant repository</param>
        /// <param name="notificationClient">notification client</param>
        public SprintParticipantService(ISprintParticipantRepo sprintParticipantRepo, INotificationClient notificationClient)
        {
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.NotificationClient = notificationClient;
        }

        private ISprintParticipantRepo SprintParticipantRepo { get; }

        private INotificationClient NotificationClient { get; }

        /// <summary>
        /// Mark the attendece for the given sprint and notify with evnet
        /// EventName.MarkedAttenence with MarkAttendenceMessage message
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for for participant</param>
        public async Task MarkAttendence(int sprintId, int userId)
        {
            var result = await this.SprintParticipantRepo.MarkAttendence(sprintId, userId);
            this.NotificationClient.SprintNotificationJobs.SprintMarkAttendace(
                sprintId,
                userId,
                result.Name,
                result.ProfilePicture,
                result.Country,
                result.CountryCode,
                result.City,
                result.ColorCode);
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
                var result = await this.SprintParticipantRepo.CheckSprintParticipant(sprintId, userId);

                if (result == null)
                {
                    await this.SprintParticipantRepo.AddSprintParticipant(sprintId, userId);

                    this.SprintParticipantRepo.SaveChanges();
                    return;
                }
                else
                {
                    throw new Application.ApplicationException((int)ApplicationErrorCode.BadRequest, "Duplicate participant");
                }

            }
            catch (Application.ApplicationException ex)
            {
                throw new Application.ApplicationException(ex.ErrorCode, ex.Message);
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
                this.NotificationClient.SprintNotificationJobs.SprintExit(
                    participant.SprintId,
                    participant.SprintName,
                    participant.UserId,
                    participant.UserName,
                    participant.ProfilePicture);
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
                    p.User.ColorCode,
                    p.Sprint.Id,
                    p.Sprint.Name);
                participantInfos.Add(participant);
            });
            return participantInfos;
        }

        /// <summary>
        /// Get all sprint info with given filters
        /// </summary>
        /// <param name="userId">participant id</param>
        /// <param name="sprintType"><see cref="SprintType"> sprint type</see></param>
        /// <param name="stage"><see cref="ParticipantStage"> participant stage</see></param>
        /// <param name="distanceFrom">distance in meters from</param>
        /// <param name="distanceTo">distance in meters from</param>
        /// <param name="startFrom">start from time in minutes</param>
        /// <param name="currentTimeBuff">current time difference</param>
        /// <returns><see cref="SprintInfo"> sprint info </see> </returns>
        public List<SprintInfo> GetSprints(
            int userId,
            SprintType? sprintType,
            ParticipantStage? stage,
            int? distanceFrom,
            int? distanceTo,
            int? startFrom,
            int? currentTimeBuff)
        {
            var time = DateTime.UtcNow;
            var now = DateTime.UtcNow;
            if (currentTimeBuff != null)
            {
                now = DateTime.UtcNow.AddMinutes((int)currentTimeBuff);
            }

            if (startFrom != null)
            {
                time = time.AddHours((int)startFrom);
            }

            Expression<Func<SprintParticipant, bool>> query = s =>
                s.UserId == userId &&
                (s.Sprint.Type == (int)sprintType || sprintType == null) &&
                (s.Stage == stage || stage == null) &&
                (s.Sprint.Distance >= distanceFrom || distanceFrom == 0) &&
                (s.Sprint.Distance <= distanceTo || distanceTo == 0) &&
                ((s.Sprint.StartDateTime <= time && s.Sprint.StartDateTime > now) || startFrom == 0) &&
                (s.Sprint.StartDateTime > now);
            var sprints = this.SprintParticipantRepo.GetAll(query).ToList();
            List<SprintInfo> sprintInfo = new List<SprintInfo>();
            sprints.ForEach(s =>
            {
                var sprint = new SprintInfo(
                    s.Sprint.Id,
                    s.Sprint.Name,
                    s.Sprint.Distance,
                    s.Sprint.StartDateTime,
                    s.Sprint.Type,
                    s.Sprint.CreatedBy.Id == s.UserId
                );
                sprintInfo.Add(sprint);
            });
            return sprintInfo;
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
                s.Stage == ParticipantStage.MARKED_ATTENDENCE;
            var markedAttendaceDetails = await this.SprintParticipantRepo.Get(query);
            if (markedAttendaceDetails != null)
            {
                return new SprintInfo(
                    markedAttendaceDetails.Sprint.Id,
                    markedAttendaceDetails.Sprint.Name,
                    markedAttendaceDetails.Sprint.Distance,
                    markedAttendaceDetails.Sprint.StartDateTime,
                    markedAttendaceDetails.Sprint.Type);
            }
            else
            {
                throw new Application.ApplicationException("NOT_FOUND_MARKED_ATTENDACE");
            }
        }

        public async Task<SprintParticipantDto> SprintInvite(int sprintId, int inviterId, int inviteeId)
        {
            await this.SprintParticipantRepo.AddParticipant(sprintId, inviteeId);
            var sprint = await this.SprintParticipantRepo.GetSprint(sprintId);
            var invitee = await this.SprintParticipantRepo.GetParticipant(inviteeId);
            this.NotificationClient.SprintNotificationJobs.SprintInvite(sprintId, inviterId, inviteeId);
            this.SprintParticipantRepo.SaveChanges();
            return new SprintParticipantDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                invitee.Id,
                invitee.Name,
                invitee.ProfilePicture,
                invitee.City,
                invitee.Country,
                invitee.CountryCode);
        }

        public async Task<dynamic> GetNotification(int userId)
        {
            var notifications = this.SprintParticipantRepo.GetNotification(userId);
            var result = new List<object>();

            notifications.ToList().ForEach(s =>
            {
                switch (s)
                {
                    case SprintNotification a:
                        result.Add(new { SprintType = NotificationDto.BuildNotification(a) });
                        break;
                    default:
                        break;
                }
            });
            return result;
        }

    }
}