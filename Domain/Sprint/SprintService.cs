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
            int tenToTwenty = this.FilterWithDistance(allSprints, 10, 20).Count();
            int twentyOneToThirty = this.FilterWithDistance(allSprints, 21, 30).Count();
            return new LiveSprintCount(all, tenToTwenty, twentyOneToThirty);
        }

        /// <summary>
        /// Update instance of SprintService
        /// </summary>
        /// <param name="sprintData">sprint repository</param>
        public async Task<Sprint> UpdateSprint(SprintModel sprintData)
        {
            Sprint updateSprint = new Sprint();
            updateSprint.Id = sprintData.Id;

            var sprintAavail = await this.SprintRepo.GetSprint(updateSprint.Id);
            sprintAavail.Name = sprintData.Name;
            sprintAavail.Distance = sprintData.Distance;
            sprintAavail.StartDateTime = sprintData.StartTime;
            sprintAavail.Type = sprintData.SprintType;
            sprintAavail.LocationProvided = sprintData.LocationProvided;
            sprintAavail.Lattitude = sprintData.Lattitude;
            sprintAavail.Longitutude = sprintData.Longitutude;
            sprintAavail.NumberOfParticipants = sprintData.NumberOfParticipants;
            var value = sprintAavail.Id;
            if (sprintAavail != null)
            {
                Sprint sprint = await this.SprintRepo.UpdateSprint(sprintAavail);
                if (sprint != null)
                {
                    this.SprintRepo.SaveChanges();
                }

                return sprint;
            }

            return null;
        }

        /// <summary>
        /// creates a new sprint
        /// </summary>
        /// <param name="sprintInfo">info about the sprint</param>
        /// <param name="ownerOfSprint">user who created the sprint</param>
        /// <returns>created sprint</returns>
        public async Task<Sprint> CreateNewSprint(SprintModel sprintInfo, User ownerOfSprint)
        {
            Sprint sprintToBeCreated = new Sprint();
            sprintToBeCreated.CreatedBy = ownerOfSprint;
            sprintToBeCreated.Type = sprintInfo.SprintType;
            sprintToBeCreated.LocationProvided = sprintInfo.LocationProvided;
            sprintToBeCreated.Lattitude = sprintInfo.Lattitude;
            sprintToBeCreated.Longitutude = sprintInfo.Longitutude;
            sprintToBeCreated.Name = sprintInfo.Name;
            sprintToBeCreated.StartDateTime = sprintInfo.StartTime;
            sprintToBeCreated.Status = (int)SprintStatus.NOTSTARTEDYET;
            sprintToBeCreated.Distance = sprintInfo.Distance;
            sprintToBeCreated.NumberOfParticipants = sprintInfo.NumberOfParticipants;
            Sprint sprint = await this.SprintRepo.AddSprint(sprintToBeCreated);
            if (sprint != null)
            {
                this.SprintRepo.SaveChanges();
            }

            return sprint;
        }

        private List<Sprint> FilterWithDistance(List<Sprint> sprints, int from, int to)
        {
            return sprints
                .Where(s => s.Distance >= from * 1000 && s.Distance <= to * 1000).ToList();
        }
    }
}