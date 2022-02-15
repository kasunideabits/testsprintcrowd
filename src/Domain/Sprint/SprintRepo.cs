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
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Domain.Sprint.Dtos;

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
        /// Get Last Special Sprint
        /// </summary>
        /// <returns></returns>
        public async Task<Sprint> GetLastSpecialSprint()
        {
            return await this.dbContext.Set<Sprint>().OrderByDescending(p => p.Id).FirstOrDefaultAsync(p => !String.IsNullOrEmpty(p.PromotionCode));
        }

        /// <summary>
        /// Get Last Special Sprint Program
        /// </summary>
        /// <returns></returns>
        public async Task<SprintProgram> GetLastSpecialSprintProgram()
        {
            return await this.dbContext.Set<SprintProgram>().OrderByDescending(p => p.Id).FirstOrDefaultAsync(p => !String.IsNullOrEmpty(p.ProgramCode));
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

        /// <summary>
        /// Get all sprints with given predicate
        /// </summary>
        /// <param name="predicate">query </param>
        /// <returns>all sprints match to predicate</returns>
        public IEnumerable<Sprint> GetSprint_Open(Expression<Func<Sprint, bool>> predicate)
        {
            return this.dbContext.Sprint.Include(s => s.Participants).ThenInclude(s => s.User).Where(predicate);

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
        /// <param name="pageNo">No of the page</param>
        /// <param name="limit">No of items per page</param>
        /// <returns>all events with given type</returns>
        public async Task<SprintsPageDto> GetAllEvents(int eventType, string searchTerm, string sortBy, string filterBy, int pageNo, int limit , List<RolesDto> userRoles, int userId)
        {

            IQueryable<Sprint> allEvents = null;
            List<Sprint> sprints = new List<Sprint>();
            int noOfItems = 0;
            if (Regex.IsMatch(searchTerm, @"^(?:19|20)\d{2}$") || Regex.IsMatch(searchTerm, @"^(19|20)\d\d[- /.](0[1-9]|1[012])$") || Regex.IsMatch(searchTerm, @"^(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$"))
            {
                allEvents = (from sprint in this.dbContext.Sprint
                             where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                  (searchTerm.Equals("null") ||
                                          (
                                          sprint.StartDateTime.ToString().StartsWith(searchTerm.Trim())
                                          )
                                  )
                             )
                             select sprint
                     );

                // return allEvents;
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

                allEvents = (from sprint in this.dbContext.Sprint
                             where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                          (
                                              sprint.StartDateTime.ToString().Contains(subtractedTime.ToString().Trim())
                                              || sprint.StartDateTime.ToString().Contains(subtractedtwentyHrTime.ToString().Trim())
                                              || sprint.StartDateTime.ToString().Contains(subtractedtwentyHrTimePM.ToString().Trim())
                                          )
                             )
                             select sprint
                     );


                // return allEvents;
            }
            else
            {//if entered keyword is not a time format, following executes
                allEvents = (from sprint in this.dbContext.Sprint
                             where ((sprint.Type == eventType) && (sprint.Status != 3) &&
                                  (searchTerm.Equals("null") ||
                                          (
                                          sprint.Name.ToLower().Contains(searchTerm.Trim().ToLower()) ||
                                          sprint.InfluencerEmail.ToLower().Contains(searchTerm.Trim().ToLower())
                                          )
                                  )
                             )
                             select sprint
                    );

                // return allEvents;
            }

            IQueryable<Sprint> allEventsFilter = null;
            try
            {
                if (userRoles.Any(item => item.RoleName != Enums.UserRoles.Admin) && allEvents != null && allEvents.Count() != 0)
                {
                    allEventsFilter = allEvents.Where(spr => spr.CreatedBy.Id==userId);
                    noOfItems = allEventsFilter.Count();
                }
                else
                {
                    noOfItems = allEvents.Count();
                    allEventsFilter = allEvents;
                }
                if (limit > 0)
                {
                    sprints = await allEventsFilter.OrderByDescending(x => x.CreatedDate).Skip(pageNo * limit).Take(limit).ToListAsync();
                }
                else
                {
                    sprints = await allEventsFilter.OrderByDescending(x => x.CreatedDate).ToListAsync();
                }

                return new SprintsPageDto()
                {
                    sprints = sprints,
                    totalItems = noOfItems
                };
            }
            catch (Exception ex)
            {
                throw ex;
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
            if (sprintToAdd.Interval == 0)
            {
                sprintToAdd.Interval = 15;
            }

            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            this.dbContext.SaveChanges();
            return result.Entity;
        }

        /// <summary>
        /// create events
        /// </summary>
        /// <param name="eventsToCreate">list of sprints to be created</param>
        public async Task AddMultipleSprints(IEnumerable<Sprint> eventsToCreate)
        {
            foreach (var sprint in eventsToCreate)
            {
                if (sprint.Interval == 0)
                {
                    sprint.Interval = 15;
                }
            }

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
            if (sprintToAdd.Interval == 0)
            {
                sprintToAdd.Interval = 15;
            }

            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            return result.Entity;
        }

        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="sprintData">sprint repository</param>
        public async Task<Sprint> UpdateSprint(Sprint sprintData)
        {
            if (sprintData.Interval == 0)
            {
                sprintData.Interval = 15;
            }

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
            this.dbContext.SaveChanges();
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

        /// <summary>
        /// Find Influencer
        /// </summary>
        /// <param name="influencerEmail"></param>
        /// <returns></returns>
        public async Task<User> FindInfluencer(string influencerEmail)
        {
            var result = await this.dbContext.User.FirstOrDefaultAsync(u => u.Email.Trim() == influencerEmail.Trim());
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

        /// <summary>
        /// Update Sprint Status By SprintId
        /// </summary>
        /// <param name="sprintId"></param>
        public int UpdateSprintStatusBySprintId(int sprintId)
        {
            var userNotification = this.dbContext.Sprint.Where(s => s.Id == sprintId).ToList().FirstOrDefault();
            userNotification.Status = (int)SprintStatus.INPROGRESS;
            this.dbContext.Sprint.Update(userNotification);
            return this.dbContext.SaveChanges();
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="sprintToAdd">event model</param>
        /// <returns>add sprint Program</returns>
        public async Task<SprintProgram> AddSprintProgram(SprintProgram sprintProgram)
        {
            
            var result = await this.dbContext.SprintProgram.AddAsync(sprintProgram);
            this.dbContext.SaveChanges();
            return result.Entity;
        }


        /// <summary>
        /// Update Sprint Program Data
        /// </summary>
        /// <param name="sprintProgramData"></param>
        /// <returns></returns>
        public async Task<SprintProgram> UpdateSprintProgram(SprintProgram sprintProgramData)
        {
            try
            {
                var result = this.dbContext.SprintProgram.Update(sprintProgramData);
                this.dbContext.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get All User Mails
        /// </summary>
        /// <returns></returns>
        public async Task<SprintProgram> GetSprintProgramDetailsByUser(int userId, int sprintProgramId)
        {
            try
            {
                return await this.dbContext.SprintProgram.FirstOrDefaultAsync(sp => sp.Id == sprintProgramId);
            }
            catch (System.Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Get All Sprint Program For Dashboard
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<SprintProgramsPageDto> GetAllSprintProgramForDashboard(int pageNo, int limit)
        {
            int intervel = 10;
            var sprintPrograms = await (this.dbContext.SprintProgram.Where(s => s.StartDate > DateTime.UtcNow && (s.IsPrivate == false || s.IsPromoteInApp == true))).ToListAsync();

            return new SprintProgramsPageDto()
            {
                sPrograms = sprintPrograms.Skip(pageNo * limit).Take(limit).ToList(),
                totalItems = sprintPrograms.Count()
            };
        }


        /// <summary>
        /// Get All Sprint Programms
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public async Task<SprintProgramsPageDto> GetAllSprintProgramms(int userId, string searchTerm, int pageNo, int limit, List<RolesDto> userRoles)
        {
            try
            {
                IQueryable<SprintProgram> allEvents = null;
                List<SprintProgram> sprintPrograms = new List<SprintProgram>();
                int noOfItems = 0;
                if (Regex.IsMatch(searchTerm, @"^(?:19|20)\d{2}$") || Regex.IsMatch(searchTerm, @"^(19|20)\d\d[- /.](0[1-9]|1[012])$") || Regex.IsMatch(searchTerm, @"^(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$"))
                {
                    allEvents = (from sprintProgram in this.dbContext.SprintProgram
                                 where ((sprintProgram.Status != 3) && (searchTerm.Equals("null") || sprintProgram.StartDate.ToString().StartsWith(searchTerm.Trim())))select sprintProgram);

                    // return allEvents;
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

                    allEvents = (from sprintProgram in this.dbContext.SprintProgram
                                 where ((sprintProgram.Status != 3) &&
                                            (sprintProgram.StartDate.ToString().Contains(subtractedTime.ToString().Trim())
                                            || sprintProgram.StartDate.ToString().Contains(subtractedtwentyHrTime.ToString().Trim())
                                            || sprintProgram.StartDate.ToString().Contains(subtractedtwentyHrTimePM.ToString().Trim()))
                                            
                                 )
                                 select sprintProgram
                         );


                    // return allEvents;
                }
                else
                {//if entered keyword is not a time format, following executes
                    allEvents = (from sprintProgram in this.dbContext.SprintProgram
                                 where ((sprintProgram.Status != 3) && (searchTerm.Equals("null") ||
                                 (
                                              sprintProgram.Name.ToLower().Contains(searchTerm.Trim().ToLower())
                                              )
                                      )
                                 )
                                 select sprintProgram
                        );

                    // return allEvents;
                }
                IQueryable<SprintProgram> allEventsFilter = null;
               
                    if (userRoles.Any(item => item.RoleName != Enums.UserRoles.Admin) && allEvents != null && allEvents.Count() != 0)
                    {
                        allEventsFilter = allEvents.Where(spr => spr.CreatedBy.Id == userId);
                        noOfItems = allEventsFilter.Count();
                    }
                    else
                    {
                        noOfItems = allEvents.Count();
                        allEventsFilter = allEvents;
                    }
                    if (limit > 0)
                    {
                    sprintPrograms = await allEventsFilter.OrderByDescending(x => x.CreatedDate).Skip(pageNo * limit).Take(limit).ToListAsync();
                    }
                    else
                    {
                    sprintPrograms = await allEventsFilter.OrderByDescending(x => x.CreatedDate).ToListAsync();
                    }

                    return new SprintProgramsPageDto()
                    {
                        sPrograms = sprintPrograms,
                        totalItems = noOfItems
                    };
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get All Sprint Programms Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetAllSprintProgrammsCount(int userId)
        {
            try
            {
                return (this.dbContext.SprintProgram.Where(s => s.CreatedBy.Id == userId )).ToListAsync().Result.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Update Sprint Program Data
        /// </summary>
        /// <param name="programData"></param>
        /// <returns></returns>
        public async Task<SprintProgram> UpdateSprintProgramData(SprintProgram programData)
        {
           
            var result = this.dbContext.SprintProgram.Update(programData);
            this.dbContext.SaveChanges();
            return result.Entity;

        }

        
        /// <summary>
        /// Get sprint program by given predicate
        /// </summary>
        /// <param name="predicate"> predicate</param>
        public async Task<SprintProgram> GetSprintProgram(Expression<Func<SprintProgram, bool>> predicate)
        {
            return await this.dbContext.Set<SprintProgram>().FirstOrDefaultAsync(predicate);

        }


        /// <summary>
        /// Get Program Sprint List By Program Id
        /// </summary>
        /// <returns></returns>
        public async Task<List<Sprint>> GetProgramSprintListByProgramId(int sprintProgramId)
        {
            try
            {
                var program = this.dbContext.SprintProgram.Where(sp => sp.Id == sprintProgramId ).FirstOrDefaultAsync();
                return await this.dbContext.Sprint.Where(sp => sp.ProgramId == sprintProgramId && sp.StartDateTime > program.Result.StartDate).ToListAsync();
            }
            catch (System.Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Get Program Sprint List By SprintStartDate
        /// </summary>
        /// <returns></returns>
        public async Task<List<SprintProgram>> GetProgramSprintListBySprintStartDate(DateTime sprintStartDate)
        {
            try
            {
                return await this.dbContext.SprintProgram.Where(sp => sp.StartDate < sprintStartDate && sp.StartDate.AddDays(sp.Duration * 7) > sprintStartDate).ToListAsync();
            }
            catch (System.Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Get All Scheduled Program Detail For Dashboard
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<List<ProgramSprintScheduleDto>>> GetAllScheduledProgramsDetail(int programId, int pageNo, int limit)
        {
            var program = this.GetSprintProgramDetailsByUser(0, programId);
            
            List<List<ProgramSprintScheduleDto>> programSprintSchedule2 = new List<List<ProgramSprintScheduleDto>>();

            int weeks = 1;
            

            while (weeks <= program.Result.Duration)
            {
                List<ProgramSprintScheduleDto> programSprintSchedule = new List<ProgramSprintScheduleDto>();

                var programSprints = await this.dbContext.Sprint.Where(s => s.ProgramId == programId && ((DateTime.ParseExact(program.Result.StartDate.AddDays((weeks-1)*7).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) <= DateTime.ParseExact(s.StartDateTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)) && (DateTime.ParseExact(s.StartDateTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) <= DateTime.ParseExact(program.Result.StartDate.AddDays(weeks*7).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)))).ToListAsync();

                DateTime StartDateOfWeek = DateTime.ParseExact(program.Result.StartDate.AddDays((weeks - 1) * 7).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                for (int weekDays = 0; weekDays < 7; weekDays++)
                {
                    ProgramSprintScheduleDto objSprintSchedule = new ProgramSprintScheduleDto();
                    foreach (Sprint sprint in programSprints)
                    {

                        if (StartDateOfWeek.AddDays(weekDays).ToString("dd/MM/yyyy") == sprint.StartDateTime.ToString("dd/MM/yyyy"))
                        {
                            objSprintSchedule.ProgramSprints.Add(new ProgramSprint(sprint.Id, sprint.Name, sprint.Distance, sprint.StartDateTime, sprint.ImageUrl));

                        }
                        //objSprintSchedule.WeekDate = StartDateOfWeek.AddDays(weekDays);
                    }
                    objSprintSchedule.WeekDate = StartDateOfWeek.AddDays(weekDays);
                    programSprintSchedule.Add(objSprintSchedule);
                }

                programSprintSchedule2.Add(programSprintSchedule);
                //objSprintSchedule.ProgramSprints = null;
                weeks++;
            }

            return programSprintSchedule2;
        }
    }
}
