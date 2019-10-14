namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
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
        public SprintService(ISprintRepo sprintRepo)
        {
            this.SprintRepo = sprintRepo;
        }

        private ISprintRepo SprintRepo { get; }

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
            int sprintId,
            string name,
            int? distance,
            DateTime? startTime,
            int? numberOfParticipants,
            string influencerEmail,
            int? draftEvent)
        {
            var sprintAavail = await this.SprintRepo.GetSprint(sprintId);
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
            int draft)
        {
            Sprint sprint = new Sprint();
            sprint.Name = name;
            sprint.Distance = distance;
            sprint.StartDateTime = startTime;
            sprint.CreatedBy = user;
            sprint.Type = type;
            sprint.Status = (int)SprintStatus.NOTSTARTEDYET;
            sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
            sprint.InfluencerAvailability = infulenceEmail != null;
            sprint.InfluencerEmail = infulenceEmail;
            sprint.DraftEvent = draft;
            Sprint addedSprint = await this.SprintRepo.AddSprint(sprint);

            this.SprintRepo.SaveChanges();

            CreateSprintDto result = new CreateSprintDto(
                addedSprint.Id,
                addedSprint.Name,
                addedSprint.Distance,
                addedSprint.NumberOfParticipants,
                addedSprint.StartDateTime,
                (SprintType)addedSprint.Type,
                addedSprint.DraftEvent,
                addedSprint.InfluencerAvailability,
                addedSprint.InfluencerEmail);

            return result;

        }

        /// <summary>
        /// Get the sprint details and sprint participant details with given
        /// sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <returns><see cref="SprintWithPariticpants">sprint details</see></returns>
        public async Task<SprintWithPariticpants> GetSprintWithPaticipants(int sprintId)
        {
            var sprint = await this.SprintRepo.GetSprintWithPaticipants(sprintId);
            SprintWithPariticpants result = new SprintWithPariticpants(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                sprint.Location);
            sprint.Participants
                .ForEach(p =>
                {
                    result.AddParticipant(
                        p.User.Id,
                        p.User.Name,
                        p.User.ProfilePicture,
                        p.User.City,
                        p.User.Country,
                        p.User.CountryCode,
                        p.User.Id == sprint.CreatedBy.Id);
                });
            return result;
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
    }
}