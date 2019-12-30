namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Sprint service
    /// </summary>
    public class SprintService : ISprintService
    {
        /// <summary>
        /// initializes an instance of SprintService
        /// </summary>
        /// <param name="sprintRepo">sprint repository</param>
        /// <param name="notificationClient">notification client</param>
        public SprintService(ISprintRepo sprintRepo, INotificationClient notificationClient)
        {
            this.SprintRepo = sprintRepo;
            this.NotificationClient = notificationClient;
        }

        private ISprintRepo SprintRepo { get; }
        private INotificationClient NotificationClient { get; }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>Available all events</returns>
        public async Task<List<Sprint>> GetAll(int eventType)
        {
            return await this.SprintRepo.GetAllEvents(eventType);
        }

        /// <summary>
        /// Get created sprint count for given date range
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>Created All, Public, Private sprints</returns>
        public async Task<CreatedSprintCount> GetCreatedEventsCount(DateTime? from, DateTime? to)
        {
            List<Sprint> allSprints = new List<Sprint>();
            if (from.HasValue && to.HasValue)
            {
                allSprints = await this.SprintRepo.GetAllEvents(from.Value, to.Value);
            }
            else
            {
                allSprints = await this.SprintRepo.GetAllEvents();
            }

            int totalCount = allSprints.Count();
            int privateCount = allSprints.Where(s => s.Type == (int)SprintType.PrivateSprint).Count();
            int publicCount = allSprints.Where(s => s.Type == (int)SprintType.PublicSprint).Count();
            return new CreatedSprintCount(totalCount, privateCount, publicCount);
        }

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>Toatal count of live events, 10-20KM and 21-30km</returns>
        public async Task<LiveSprintCount> GetLiveSprintCount()
        {
            List<Sprint> allSprints = await this.SprintRepo.GetLiveSprints();
            int all = allSprints.Count();
            int twoToTen = this.FilterWithDistance(allSprints, 2, 10).Count();
            int tenToTwenty = this.FilterWithDistance(allSprints, 10, 20).Count();
            int twentyOneToThirty = this.FilterWithDistance(allSprints, 21, 30).Count();
            return new LiveSprintCount(all, twoToTen, tenToTwenty, twentyOneToThirty);
        }

        /// <summary>
        /// Update instance of SprintService
        /// </summary>
        public async Task<UpdateSprintDto> UpdateSprint(
            int userId,
            int sprintId,
            string name,
            int? distance,
            DateTime? startTime,
            int? numberOfParticipants,
            string influencerEmail,
            int? draftEvent)
        {
            Expression<Func<Sprint, bool>> predicate = s => s.Id == sprintId;
            var sprintAavail = await this.SprintRepo.GetSprint(predicate);
            if (sprintAavail == null)
            {
                throw new Application.ApplicationException((int)SprintErrorCode.NotMatchingSprintWithId, "Sprint not found");
            }
            else if (sprintAavail.CreatedBy.Id != userId)
            {
                throw new Application.ApplicationException((int)SprintErrorCode.NotAllowedOperation, "Only creator can edit event");
            }
            string oldName = sprintAavail.Name;
            if (name != String.Empty)
            {
                sprintAavail.Name = name;
            }
            if (distance != null)
            {
                sprintAavail.Distance = (int)distance;
            }
            if (startTime != null)
            {
                sprintAavail.StartDateTime = (DateTime)startTime;
            }
            if (distance != null)
            {
                sprintAavail.Distance = (int)distance;
            }
            if (numberOfParticipants != null)
            {
                sprintAavail.NumberOfParticipants = (int)numberOfParticipants;
            }
            if (influencerEmail != String.Empty)
            {
                sprintAavail.InfluencerAvailability = true;
                sprintAavail.InfluencerEmail = influencerEmail;
            }
            if (draftEvent != null)
            {
                sprintAavail.DraftEvent = (int)draftEvent;
            }
            Sprint sprint = await this.SprintRepo.UpdateSprint(sprintAavail);
            this.SprintRepo.SaveChanges();
            this.NotificationClient.SprintNotificationJobs.SprintUpdate(
                sprint.Id,
                oldName,
                sprint.Name,
                sprint.Distance,
                sprint.StartDateTime,
                sprint.NumberOfParticipants,
                (SprintStatus)sprint.Status,
                (SprintType)sprint.Type,
                sprint.CreatedBy.Id
            );
            UpdateSprintDto result = new UpdateSprintDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                sprint.DraftEvent,
                sprint.InfluencerAvailability,
                sprint.InfluencerEmail);
            return result;
        }

        /// <summary>
        /// creates a new sprint
        /// </summary>
        public async Task<CreateSprintDto> CreateNewSprint(
            User user,
            string name,
            int distance, DateTime startTime,
            int type,
            int? numberOfParticipants,
            string infulenceEmail,
            int draft,
            bool influencerAvailability)
        {
            if (type == (int)SprintType.PrivateSprint)
            {
                Expression<Func<Sprint, bool>> predicate = s => s.CreatedBy.Id == user.Id && s.StartDateTime > DateTime.UtcNow && s.Status != (int)SprintStatus.ARCHIVED;
                var isAlreadyCreatedSprint = await this.SprintRepo.GetSprint(predicate);
                if (isAlreadyCreatedSprint != null)
                {
                    throw new SCApplicationException((int)SprintErrorCode.AlreadyExistSprint, "Already exist event");
                }
            }
            Sprint sprint = new Sprint();
            sprint.Name = name;
            sprint.Distance = distance;
            sprint.StartDateTime = startTime;
            sprint.CreatedBy = user;
            sprint.Type = type;
            sprint.Status = (int)SprintStatus.NOTSTARTEDYET;
            sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
            sprint.InfluencerAvailability = influencerAvailability;
            sprint.InfluencerEmail = infulenceEmail;
            sprint.DraftEvent = draft;
            Sprint addedSprint = await this.SprintRepo.AddSprint(sprint);

            if (type == (int)SprintType.PrivateSprint)
            {
                await this.SprintRepo.AddParticipant(user.Id, addedSprint.Id, ParticipantStage.JOINED);
            }

            this.SprintRepo.SaveChanges();

            return CreateSprintDtoMapper(sprint, user);
        }

        /// <summary>
        /// Get sprint with pariticipants by creator id
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="extendedTime"> extended time </param>
        /// <returns><see cref="SprintWithPariticpantsDto"> sprint details with paritipants</see></returns>
        public async Task<SprintWithPariticpantsDto> GetSprintByCreator(int userId, int? extendedTime)
        {
            var now = extendedTime == null ? DateTime.UtcNow : DateTime.UtcNow.AddMinutes((int)extendedTime);
            Expression<Func<Sprint, bool>> predicate = s => s.CreatedBy.Id == userId && s.Status != (int)SprintStatus.ARCHIVED && s.StartDateTime > now;
            var sprint = await this.SprintRepo.GetSprint(predicate);
            if (sprint == null)
            {
                return null;
            }
            else
            {
                Expression<Func<SprintParticipant, bool>> participantPredicate = s => s.SprintId == sprint.Id && s.User.UserState == UserState.Active;
                var participants = this.SprintRepo.GetParticipants(participantPredicate);
                return SprintWithPariticpantsMapper(sprint, participants.ToList());
            }
        }

        /// <summary>
        /// Get the sprint details and sprint participant details with given
        /// sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        public async Task<SprintWithPariticpantsDto> GetSprintWithPaticipants(int sprintId)
        {
            Expression<Func<Sprint, bool>> sprintPredicate = s => s.Id == sprintId;
            var sprint = await this.SprintRepo.GetSprint(sprintPredicate);
            Expression<Func<SprintParticipant, bool>> participantPredicate = s => s.SprintId == sprintId && s.User.UserState == UserState.Active;
            var pariticipants = this.SprintRepo.GetParticipants(participantPredicate);
            return SprintWithPariticpantsMapper(sprint, pariticipants.ToList());
        }

        /// <summary>
        /// Remove sprint
        /// </summary>
        /// <param name="userId">creator id </param>
        /// <param name="sprintId">sprint id to remove</param>
        public async Task Remove(int userId, int sprintId)
        {
            Expression<Func<Sprint, bool>> sprintPredicate = s => s.Id == sprintId;
            var sprint = await this.SprintRepo.GetSprint(sprintPredicate);
            if (sprint == null)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotMatchingSprintWithId, "Sprint not found with given id");
            }
            else if (sprint.CreatedBy.Id != userId)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotAllowedOperation, "Only creator can delete event");
            }
            else if (sprint.StartDateTime.AddMinutes(-10) < DateTime.UtcNow)
            {
                throw new SCApplicationException((int)SprintErrorCode.MarkAttendanceEnable, "Mark attendance enable");
            }
            else
            {
                sprint.Status = (int)SprintStatus.ARCHIVED;
                await this.SprintRepo.UpdateSprint(sprint);
                this.SprintRepo.SaveChanges();
                this.NotificationClient.SprintNotificationJobs.SprintRemove(
                    sprint.Id,
                    sprint.Name,
                    sprint.Distance,
                    sprint.StartDateTime,
                    sprint.NumberOfParticipants,
                    (SprintStatus)sprint.Status,
                    (SprintType)sprint.Type,
                    sprint.CreatedBy.Id,
                    sprint.CreatedBy.Name,
                    sprint.CreatedBy.ProfilePicture,
                    sprint.CreatedBy.Code,
                    sprint.CreatedBy.ColorCode,
                    sprint.CreatedBy.City,
                    sprint.CreatedBy.Country,
                    sprint.CreatedBy.CountryCode);
            }
        }

        public async Task InviteRequest(int inviterId, int inviteeId, int sprintId)
        {
            await this.SprintRepo.AddParticipant(inviteeId, sprintId);
        }

        private List<Sprint> FilterWithDistance(List<Sprint> sprints, int from, int to)
        {
            return sprints
                .Where(s => s.Distance >= from * 1000 && s.Distance <= to * 1000).ToList();
        }

        private static int NumberOfParticipants(int sprintType)
        {
            if (sprintType == (int)SprintType.PrivateSprint)
            {
                return 3;
            }
            if (sprintType == (int)SprintType.PublicSprint)
            {
                return 30;
            }
            throw new Application.ApplicationException("Invalid sprint type");
        }

        public static CreateSprintDto CreateSprintDtoMapper(Sprint sprint, User user)
        {
            CreateSprintDto result = new CreateSprintDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                user.Id,
                user.Name,
                user.ProfilePicture,
                user.City,
                user.Country,
                user.CountryCode,
                user.ColorCode,
                true,
                ParticipantStage.JOINED);
            return result;
        }

        public static SprintWithPariticpantsDto SprintWithPariticpantsMapper(Sprint sprint, List<SprintParticipant> participants)
        {
            SprintWithPariticpantsDto result = new SprintWithPariticpantsDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                sprint.Location);
            participants
                .ForEach(p =>
                {
                    result.AddParticipant(
                        p.User.Id,
                        p.User.Name,
                        p.User.ProfilePicture,
                        p.User.City,
                        p.User.Country,
                        p.User.CountryCode,
                        p.User.ColorCode,
                        p.User.Id == sprint.CreatedBy.Id,
                        p.Stage);
                });
            return result;
        }

        /// <summary>
        /// Get public sprint with user preference
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="timeOffset">time offset to utc</param>
        /// <returns>sprint with participant info</returns>
        public async Task<List<PublicSprintWithParticipantsDto>> GetPublicSprints(int userId, int timeOffset)
        {
            var userPreference = await this.SprintRepo.GetUserPreference(userId);
            var query = new PublicSprintQueryBuilder(userPreference).Build(timeOffset);
            IEnumerable<Sprint> sprints = await this.SprintRepo.GetSprints(query);
            var sprintDto = new List<PublicSprintWithParticipantsDto>();
            var friendsRelations = this.SprintRepo.GetFriends(userId);
            var friends = friendsRelations.Select(f => f.AcceptedUserId == userId ? f.SharedUserId : f.AcceptedUserId);
            foreach (var sprint in sprints)
            {

                var resultDto = new PublicSprintWithParticipantsDto(sprint.Id, sprint.Name, sprint.Distance, sprint.NumberOfParticipants, sprint.StartDateTime, (SprintType)sprint.Type, sprint.Location);
                var participants = sprint.Participants.Where(s => s.User.UserState == UserState.Active);
                foreach (var participant in participants)
                {
                    resultDto.AddParticipant(
                        participant.User.Id,
                        participant.User.Name,
                        participant.User.ProfilePicture,
                        participant.User.City,
                        participant.User.Country,
                        participant.User.CountryCode,
                        participant.User.ColorCode,
                        false,
                        participant.Stage,
                        friends.Contains(participant.UserId));

                }
                sprintDto.Add(resultDto);
            }
            return sprintDto;
        }
    }
}