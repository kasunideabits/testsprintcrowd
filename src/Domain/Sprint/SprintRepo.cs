namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dlos;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using System.Text.RegularExpressions;
    using System.Globalization;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;

    /// <summary>
    /// Event repositiory
    /// </summary>
    public class SprintRepo : ISprintRepo
    {
        private ScrowdDbContext dbContext;
        /// <summary>
        /// intializes an instace of EventRepo
        /// </summary>
        /// <param name="dbContext">db context</param>
        public SprintRepo(ScrowdDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get sprint by given predicate
        /// </summary>
        /// <param name="predicate"> predicate</param>
        public async Task<Sprint> GetSprint(Expression<Func<Sprint, bool>> predicate)
        {
            return await this.dbContext.Set<Sprint>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Get all sprints with given predicate
        /// </summary>
        /// <param name="predicate">query </param>
        /// <returns>all sprints match to predicate</returns>
        public async Task<IQueryable<Sprint>> GetSprints(Expression<Func<Sprint, bool>> predicate)
        {
            var result = this.dbContext.Sprint.Include(s => s.Participants).ThenInclude(s => s.User).Where(predicate);
            return result;
        }

        public async Task<List<Sprint>> GetAllEvents()
        {
            return await this.dbContext.Sprint.ToListAsync();
        }

        /// <summary>
        /// get all sprint public or private
        /// </summary>
        /// <param name="eventType">public or private</param>
        /// <param name="searchTerm">Search term to filter</param>
        /// <param name="sortBy">Sort by option</param>
        /// <param name="filterBy">Filter by option</param>
        /// <returns>all events with given type</returns>
        public async Task<List<Sprint>> GetAllEvents(int eventType, string searchTerm, string sortBy, string filterBy)
        {
            if (Regex.IsMatch(searchTerm, @"^(?:19|20)\d{2}$") || Regex.IsMatch(searchTerm, @"^(19|20)\d\d[- /.](0[1-9]|1[012])$") || Regex.IsMatch(searchTerm, @"^(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$"))
            {
                var allEvents = await (from sprint in this.dbContext.Sprint
                                       where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                            (searchTerm.Equals("null") ||
                                                    (
                                                    sprint.StartDateTime.ToString().StartsWith(searchTerm.Trim())
                                                    )
                                            )
                                       )
                                       select sprint
                     ).OrderByDescending(x => x.CreatedDate).ToListAsync();

                return allEvents;
            }
            else if (Regex.IsMatch(searchTerm, @"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$"))
            {
                TimeSpan localTime = TimeSpan.Parse(searchTerm);
                TimeSpan twentyHrTime = localTime.Add(new TimeSpan(12, 0, 0));

                //Remove following line when time zone is UTC
                TimeSpan subtractedtwentyHrTime = twentyHrTime.Subtract(new TimeSpan(5, 30, 0));
                TimeSpan subtractedtwentyHrTimePM = subtractedtwentyHrTime.Add(new TimeSpan(12, 0, 0));
                //Remove following line when time zone is UTC
                TimeSpan subtractedTime = localTime.Subtract(new TimeSpan(5, 30, 0));

                var allEventsAM = await (from sprint in this.dbContext.Sprint
                                         where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                                      (
                                                          sprint.StartDateTime.ToString().Contains(subtractedTime.ToString().Trim())
                                                      )
                                         )
                                         select sprint
                     ).OrderByDescending(x => x.CreatedDate).ToListAsync();

                var allEventsPM = await (from sprint in this.dbContext.Sprint
                                         where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                                      (
                                                          sprint.StartDateTime.ToString().Contains(subtractedtwentyHrTime.ToString().Trim())
                                                      )
                                         )
                                         select sprint
                     ).OrderByDescending(x => x.CreatedDate).ToListAsync();

                var allEventsAMPM = await (from sprint in this.dbContext.Sprint
                                           where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                                        (
                                                            sprint.StartDateTime.ToString().Contains(subtractedtwentyHrTimePM.ToString().Trim())
                                                        )
                                           )
                                           select sprint
                     ).OrderByDescending(x => x.CreatedDate).ToListAsync();

                var allEvents = allEventsAM.Union(allEventsPM).Union(allEventsAMPM).ToList();

                return allEvents;
            }
            else
            {//if entered keyword is not a time format, following executes
                var allEvents = await (from sprint in this.dbContext.Sprint
                                       where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                            (searchTerm.Equals("null") ||
                                                    (
                                                    sprint.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                    sprint.InfluencerEmail.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                                                    )
                                            )
                                       )
                                       select sprint
                    ).OrderByDescending(x => x.CreatedDate).ToListAsync();

                return allEvents;
            }
        }

        /// <summary>
        /// Get created sprint count for given date range
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>Created All, Public, Private sprints</returns>
        public async Task<List<Sprint>> GetAllEvents(DateTime from, DateTime to)
        {
            return await this.dbContext.Sprint
                .Where(s => s.CreatedDate >= from && s.CreatedDate <= to)
                .ToListAsync();
        }

        /// <summary>
        /// Get all sprint names which matches paramter
        /// </summary>
        /// <param name="sprintName">sprint name to filter</param>
        /// <returns>Return all sprint names which matches given parameter</returns>
        public async Task<List<String>> GetSprintNames(string sprintName)
        {
            return await this.dbContext.Sprint
                .Where(x => x.Name.StartsWith(sprintName) && x.Name.Contains("(") && x.Name != sprintName && x.Name.EndsWith(")") && x.Name.Length <= sprintName.Length + 3)
                .Select(n => n.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get all ongoing sprint sprints
        /// </summary>
        /// <returns>All ongoing sprints which not completed 24H</returns>
        public async Task<List<Sprint>> GetLiveSprints()
        {
            return await this.dbContext.Sprint
                .Where(s => s.Status == (int)SprintStatus.INPROGRESS)
                .ToListAsync();
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="sprintToAdd">event model</param>
        /// <returns>added sprint result</returns>
        public async Task<Sprint> AddSprint(Sprint sprintToAdd)
        {
            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            return result.Entity;
        }

        /// <summary>
        /// create events
        /// </summary>
        /// <param name="eventsToCreate">list of sprints to be created</param>
        public async Task AddMultipleSprints(IEnumerable<Sprint> eventsToCreate)
        {
            await this.dbContext.Sprint.AddRangeAsync(eventsToCreate);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="sprintToAdd">event model</param>
        /// <returns>added sprint result</returns>
        public async Task<Sprint> DraftSprint(Sprint sprintToAdd)
        {
            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            return result.Entity;
        }

        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="sprintData">sprint repository</param>
        public async Task<Sprint> UpdateSprint(Sprint sprintData)
        {
            var result = this.dbContext.Sprint.Update(sprintData);
            this.dbContext.SaveChanges();
            return result.Entity;
        }

        /// <summary>
        /// Get the participants with given predicate
        /// sprint id
        /// </summary>
        /// <param name="predicate">predicate for lookup</param>
        /// <returns><see cref="SprintParticipant">sprint pariticipants</see></returns>
        public IEnumerable<SprintParticipant> GetParticipants(Expression<Func<SprintParticipant, bool>> predicate)
        {
            return this.dbContext.SprintParticipant
                .Include(s => s.Sprint)
                .ThenInclude(s => s.CreatedBy)
                .Include(s => s.User)
                .Where(predicate);
        }

        /// <summary>
        /// Add paritipant to sprint
        /// </summary>
        /// <param name="userId">user id for pariticipant</param>
        /// <param name="sprintId">sprint id which going to join</param>
        /// <param name="participantStage">sprint participant stage</param>
        public async Task AddParticipant(int userId, int sprintId, ParticipantStage participantStage)
        {
            SprintParticipant pariticipant = new SprintParticipant()
            {
                UserId = userId,
                SprintId = sprintId,
                Stage = participantStage,
            };
            await this.dbContext.AddAsync(pariticipant);
        }

        /// <summary>
        /// Remove sprint with given id
        /// </summary>
        /// <param name="sprint">sprint entity</param>
        public void RemoveSprint(Sprint sprint)
        {
            this.dbContext.Set<Sprint>().Remove(sprint);
        }

        public async Task<UserPreference> GetUserPreference(int userId)
        {
            return await this.dbContext.UserPreferences.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Get friend list for given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>Friends</returns>
        public IEnumerable<Friend> GetFriends(int userId)
        {
            return this.dbContext.Frineds.Where(f => f.SharedUserId == userId || f.AcceptedUserId == userId);
        }

        public async Task<User> FindInfluencer(string influencerEmail)
        {
            var result = await this.dbContext.User.FirstOrDefaultAsync(u => u.Email == influencerEmail);
            return result;
        }

        /// <summary>
        /// Get SprintReportDto by timespan
        /// </summary>
        /// <param name="timespan">timespanc of the report</param>
        /// <returns>SprintReportDto</returns>
        public async Task<List<ReportItemDto>> GetReport(string timespan)
        {
            var timePeriod = DateTime.Now;
            switch (timespan)
            {
                case "week=1":
                    timePeriod = DateTime.Now.AddDays(-7);
                    break;
                case "week=2":
                    timePeriod = DateTime.Now.AddDays(-14);
                    break;
                case "month=1":
                    timePeriod = DateTime.Now.AddMonths(-1);
                    break;
                case "mlast=3":
                    timePeriod = DateTime.Now.AddMonths(-3);
                    break;
                case "mlast=6":
                    timePeriod = DateTime.Now.AddMonths(-6);
                    break;
                case "year=1":
                    timePeriod = DateTime.Now.AddYears(-1);
                    break;
            }

            int GetJoinedParticipantsBySprint(int sprintId)
            {
                var joinedParticipantsCount = (from participant in this.dbContext.SprintParticipant
                                               join sprint in this.dbContext.Sprint on participant.SprintId equals sprint.Id
                                               join user in this.dbContext.User on participant.UserId equals user.Id
                                               where ((participant.Stage == ParticipantStage.JOINED || participant.Stage == ParticipantStage.MARKED_ATTENDENCE || participant.Stage == ParticipantStage.COMPLETED || participant.Stage == ParticipantStage.QUIT) && sprint.Id == sprintId)
                                               select new { participant }
                                              ).Distinct().Count();

                return joinedParticipantsCount;
            }

            int GetMarkedAttendanceParticipantsBySprint(int sprintId)
            {
                var markedParticipantsCount = (from participant in this.dbContext.SprintParticipant
                                               join sprint in this.dbContext.Sprint on participant.SprintId equals sprint.Id
                                               join user in this.dbContext.User on participant.UserId equals user.Id
                                               where ((participant.Stage == ParticipantStage.MARKED_ATTENDENCE || participant.Stage == ParticipantStage.COMPLETED || participant.Stage == ParticipantStage.QUIT) && sprint.Id == sprintId)
                                               select new { participant }
                                              ).Distinct().Count();

                return markedParticipantsCount;
            }

            int GetCompletedParticipantsBySprint(int sprintId)
            {
                var completedParticipantsCount = (from participant in this.dbContext.SprintParticipant
                                                  join sprint in this.dbContext.Sprint on participant.SprintId equals sprint.Id
                                                  join user in this.dbContext.User on participant.UserId equals user.Id
                                                  where (participant.Stage == ParticipantStage.COMPLETED && sprint.Id == sprintId)
                                                  select new { participant }
                                              ).Distinct().Count();

                return completedParticipantsCount;
            }

            //Query for sprints for the given timespan and output as a list if sprints.
            var filteredSprintList = await (from sprint in this.dbContext.Sprint
                                            where (timePeriod <= sprint.StartDateTime && sprint.Status == 2 && sprint.Type == 0)
                                            select sprint
                                     ).ToListAsync();

            //Empty ReportItemDto list
            List<ReportItemDto> reportItemDtos = new List<ReportItemDto>();

            //Run a foreach on the above list of sprints and inside foreach bind data to the ReportItemDto object. Add the returned ReportItemDto list into another list.
            foreach (var sprint in filteredSprintList)
            {
                var completedParticipantsCount = GetCompletedParticipantsBySprint(sprint.Id);
                var rptItem = new ReportItemDto()
                {
                    SprintName = sprint.Name,
                    Distance = sprint.Distance,
                    StartDate = sprint.StartDateTime.ToLocalTime(),
                    SprintType = sprint.Type == 0 ? "Public" : "Private",
                    ParticipantsCount = (GetJoinedParticipantsBySprint(sprint.Id)),
                    ParticipantsMarkedAttendance = (GetMarkedAttendanceParticipantsBySprint(sprint.Id)),
                    ParticipantsFinishedSprint = completedParticipantsCount
                };
                reportItemDtos.Add(rptItem);
            }
            //return above returned ReportItemDto list.
            return reportItemDtos;
        }
        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }


        /// <summary>
        /// Get created sprint count for given date range
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="lapsTime"> laps Time </param>
        /// <param name="privateSprintCount"> Limit of Private sprints </param>
        /// <returns>Created All, Public, Private sprints</returns>
        public async Task<List<Sprint>> GetAllPrivateSprintsByUser(int userId, int lapsTime)
        {
            var now = lapsTime == 0 ? DateTime.UtcNow : DateTime.UtcNow.AddMinutes(lapsTime);



            return await this.dbContext.Sprint
                .Where(s => s.CreatedBy.Id == userId && s.Status != (int)SprintStatus.ARCHIVED && s.Type == (int)SprintType.PrivateSprint && s.StartDateTime > now)
                .ToListAsync();
        }

    }
}
