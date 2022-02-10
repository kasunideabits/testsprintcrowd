namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dlos;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Domain.Sprint.Dtos;

    /// <summary>
    /// inerface for event repo
    /// </summary>
    public interface ISprintRepo
    {

        /// <summary>
        /// Get sprint by given predicate
        /// </summary>
        /// <param name="predicate"> predicate</param>
        Task<Sprint> GetSprint(Expression<Func<Sprint, bool>> predicate);

        /// <summary>
        /// Get all sprints with given predicate
        /// </summary>
        /// <param name="predicate">query </param>
        /// <returns>all sprints match to predicate</returns>
        Task<IQueryable<Sprint>> GetSprints(Expression<Func<Sprint, bool>> predicate);

        /// <summary>
        /// Get all sprints
        /// </summary>
        /// <returns>all sprints</returns>
        Task<List<Sprint>> GetAllEvents();

        /// <summary>
        /// Get Last sprint that has a promotiona code
        /// </summary>
        /// <returns>sprint</returns>
        Task<Sprint> GetLastSpecialSprint();

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
        Task<SprintsPageDto> GetAllEvents(int eventType, string searchTerm, string sortBy, string filterBy, int pageNo, int limit,List<RolesDto> userRoles , int userId);

        /// <summary>
        /// Get all events with given date range
        /// </summary>
        /// <param name="from">date range from</param>
        /// <param name="to">date range to</param>
        /// <returns>All sprint with given date range</returns>
        Task<List<Sprint>> GetAllEvents(DateTime from, DateTime to);

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>All ongoing sprints</returns>
        Task<List<Sprint>> GetLiveSprints();

        /// <summary>
        /// adds new event to database
        /// </summary>
        /// <param name="eventToCreate">event model</param>
        /// <returns>Created sprint details</returns>
        Task<Sprint> AddSprint(Sprint eventToCreate);

        /// <summary>
        /// adds new events to database
        /// </summary>
        /// <param name="eventsToCreate">List of events to be created</param>
        /// <returns>Created sprint details</returns>
        Task AddMultipleSprints(IEnumerable<Sprint> eventsToCreate);

        /// <summary>
        /// adds new event to database
        /// </summary>
        /// <param name="eventToDraft">event model</param>
        /// <returns>Created sprint details</returns>
        Task<Sprint> DraftSprint(Sprint eventToDraft);

        /// <summary>
        /// Updates event in database
        /// </summary>
        /// <param name="sprintData">event model</param>
        Task<Sprint> UpdateSprint(Sprint sprintData);

        /// <summary>
        /// Get the participants with given predicate
        /// sprint id
        /// </summary>
        /// <param name="predicate">predicate for lookup</param>
        /// <returns><see cref="SprintParticipant">sprint pariticipants</see></returns>
        IEnumerable<SprintParticipant> GetParticipants(Expression<Func<SprintParticipant, bool>> predicate);

        /// <summary>
        /// Add paritipant to sprint
        /// </summary>
        /// <param name="userId">user id for pariticipant</param>
        /// <param name="sprintId">sprint id which going to join</param>
        /// <param name="participantStage">sprint participant stage</param>
        Task AddParticipant(int userId, int sprintId, ParticipantStage participantStage = ParticipantStage.PENDING);

        /// <summary>
        /// Remove sprint with given id
        /// </summary>
        /// <param name="sprint">sprint entity</param>
        void RemoveSprint(Sprint sprint);

        /// <summary>
        /// Get friend list for given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>Friends</returns>
        IEnumerable<Friend> GetFriends(int userId);

        /// <summary>
        /// Get user list for given influencerEmail
        /// </summary>
        /// <param name="influencerEmail">influencerEmail to fetch</param>
        /// <returns>User</returns>
        Task<User> FindInfluencer(string influencerEmail);


        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Get UserPreference for given userId
        /// </summary>
        /// <param name="userId">userId to fetch</param>
        /// <returns>UserPreference</returns>
        Task<UserPreference> GetUserPreference(int userId);

        /// <summary>
        /// Get all sprints by sprint name
        /// </summary>
        /// <param name="sprintName">name of the sprint</param>
        /// <returns>Friends</returns>
        Task<List<String>> GetSprintNames(string sprintName);

        /// <summary>
        /// Get ReportItemDto by timespan
        /// </summary>
        /// <param name="timespan">timespanc of the report</param>
        /// <returns>ReportItemDto</returns>
        Task<List<ReportItemDto>> GetReport(string timespan);

        /// <summary>
        /// Get new private sprint count for given user ID
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="lapsTime"> laps Time </param>

        /// <returns></returns>
        Task<List<Sprint>> GetAllPrivateSprintsByUser(int userId, int lapsTime);



        /// <summary>
        /// Update Sprint Status By SprintId
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        int UpdateSprintStatusBySprintId(int sprintId);

        /// <summary>
        /// Returen open sprints
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        IEnumerable<Sprint> GetSprint_Open(Expression<Func<Sprint, bool>> predicate);

        /// <summary>
        /// Add Sprint Program
        /// </summary>
        /// <param name="sprintProgram"></param>
        /// <returns></returns>
        Task<SprintProgram> AddSprintProgram(SprintProgram sprintProgram);

        /// <summary>
        /// Update Sprint Program
        /// </summary>
        /// <param name="sprintProgramData"></param>
        /// <returns></returns>
        Task<SprintProgram> UpdateSprintProgram(SprintProgram sprintProgramData);

        /// <summary>
        /// Get Sprint Program Details By User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sprintProgramId"></param>
         /// <returns></returns>
        Task<SprintProgram> GetSprintProgramDetailsByUser(int userId, int sprintProgramId);

        /// <summary>
        /// Get Last Special Sprint Program
        /// </summary>
        /// <returns></returns>
        Task<SprintProgram> GetLastSpecialSprintProgram();

        /// <summary>
        /// Get All Sprint Programms
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        Task<SprintProgramsPageDto> GetAllSprintProgramms(int userId, string searchTerm, int pageNo, int limit, List<RolesDto> userRoles);

        /// <summary>
        /// Get All Sprint Programms Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetAllSprintProgrammsCount(int userId);

        /// <summary>
        /// Update Sprint Program Data
        /// </summary>
        /// <param name="programData"></param>
        /// <returns></returns>
        Task<SprintProgram> UpdateSprintProgramData(SprintProgram programData);

        /// <summary>
        /// Get sprint program by given predicate
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SprintProgram> GetSprintProgram(Expression<Func<SprintProgram, bool>> predicate);

        /// <summary>
        /// Get Program Sprint List By Program Id
        /// </summary>
        /// <param name="sprintProgramId"></param>
        /// <returns></returns>
        Task<List<Sprint>> GetProgramSprintListByProgramId(int sprintProgramId);

        /// <summary>
        /// Get Program Sprint List By Sprint Start Date
        /// </summary>
        /// <param name="sprintStartDate"></param>
        /// <returns></returns>
        Task<List<SprintProgram>> GetProgramSprintListBySprintStartDate(DateTime sprintStartDate);

        /// <summary>
        /// Get All Sprint Program For Dashboard
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<SprintProgramsPageDto> GetAllSprintProgramForDashboard(int pageNo, int limit);
    }
}