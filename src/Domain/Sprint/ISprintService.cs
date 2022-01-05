namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Event;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Domain.Sprint.Dtos;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// ISprintService interface
    /// </summary>
    public interface ISprintService
    {
        /// <summary>
        /// Get all events
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="searchTerm">Search term to filter</param>
        /// <param name="sortBy">Sort to filter</param>
        /// <param name="filterBy">Term to filter</param>
        /// <param name="pageNo">No of the page</param>
        /// <param name="limit">No of items per page</param>
        /// <returns>Available events</returns>
        Task<SprintsPageDto> GetAll(int eventType, string searchTerm, string sortBy, string filterBy, int pageNo, int limit, int userId);

        /// <summary>
        /// Get created sprint count for given date range
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>Created All, Public, Private sprints</returns>
        Task<CreatedSprintCount> GetCreatedEventsCount(DateTime? from, DateTime? to);

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>Toatal count of live events, 10-20KM and 21-30km</returns>
        Task<LiveSprintCount> GetLiveSprintCount();

        /// <summary>
        /// Get Influencer Id By Email
        /// </summary>
        /// <param name="infulenceEmail"></param>
        /// <returns></returns>
        Task<int> GetInfluencerIdByEmail(string infulenceEmail);

        /// <summary>
        /// Create a new sprint, TODO : remove user object passing
        /// </summary>
        /// <param name="user">user who creating the sprint</param>
        /// <param name="name"> name for sprint</param>
        /// <param name="distance"> distance in meters for sprint</param>
        /// <param name="isSmartInvite"> select smart link or smart invite</param>
        /// <param name="startTime"> start time for sprint</param>
        /// <param name="type"><see cref="SprintType">sprint type, public or private</see></param>
        /// <param name="numberOfParticipants">number of pariticipant for sprint</param>
        /// <param name="infulenceEmail">infulence email</param>
        /// <param name="draft">sprint draft or publish</param>
        /// <param name="influencerAvailability">influencer available or not</param>
        /// <returns>cereated sprint</returns>
        Task<CreateSprintDto> CreateNewSprint(User user, CreateSprintModel sprint, TimeSpan durationForTimeBasedEvent, string descriptionForTimeBasedEvent, string repeatType, bool isCrowdRun = false);

        /// <summary>
        /// Create multiple sprints based on repeat option, TODO : remove user object passing
        /// </summary>
        /// <param name="user">user who creating the sprint</param>
        /// <param name="name"> name for sprint</param>
        /// <param name="distance"> distance in meters for sprint</param>
        /// <param name="startTime"> start time for sprint</param>
        /// <param name="type"><see cref="SprintType">sprint type, public or private</see></param>
        /// <param name="numberOfParticipants">number of pariticipant for sprint</param>
        /// <param name="infulenceEmail">infulence email</param>
        /// <param name="draft">sprint draft or publish</param>
        /// <param name="influencerAvailability">influencer available or not</param>
        /// <param name="repeatType">repeat options</param>
        /// <returns>created sprints</returns>
        Task CreateMultipleSprints(User user, CreateSprintModel sprint, TimeSpan durationForTimeBasedEvent, string repeatType, bool isCrowdRun = false);

        /// <summary>
        /// Get sprint with pariticipants by creator id
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="extendedTime"> extended time </param>
        /// <returns><see cref="SprintWithPariticpantsDto"> sprint details with paritipants</see></returns>
        Task<SprintWithPariticpantsDto> GetSprintByCreator(int userId, int? extendedTime);

        /// <summary>
        /// update sprint
        /// </summary>
        Task<UpdateSprintDto> UpdateSprint(int userId, int sprintId, CreateSprintModel sprint, TimeSpan durationForTimeBasedEvent, string descriptionForTimeBasedEvent);

        /// <summary>
        /// Validate Sprint Edit By SprintId
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        Task<bool> ValidateSprintEditBySprintId(int sprintId);

        /// <summary>
        /// Remove sprint
        /// </summary>
        /// <param name="userId">creator id </param>
        /// <param name="sprintId">sprint id to remove</param>
        Task Remove(int userId, int sprintId);

        /// <summary>
        /// Remove sprint from Admin Panel
        /// </summary>
        /// <param name="userId">creator id </param>
        /// <param name="sprintId">sprint id to remove</param>
        Task RemoveSprint(int userId, int sprintId);

        /// <summary>
        /// Get the sprint details and sprint participant details with given
        /// sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="pageNo">current page number for pagination</param>
        /// <param name="limit">limit for a page</param>

        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        Task<SprintWithPariticpantsDto> GetSprintWithPaticipants(int sprintId, int pageNo, int limit);

        /// <summary>
        /// Get Sprint Paticipants list
        /// </summary>
        /// <param name="sprintId"></param
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <param name="completed">get completed runners</param>
        /// <returns></returns>
        Task<List<SprintParticipant>> GetSprintPaticipants(int sprintId, int pageNo, int limit, bool? completed);
        Task InviteRequest(int inviterId, int inviteeId, int sprintId);

        /// <summary>
        /// Get public sprint with user preference
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="timeOffset">time offset to utc</param>
        /// <returns>sprint with participant info</returns>
        Task<List<PublicSprintWithParticipantsDto>> GetPublicSprints(int userId, int timeOffset);

        /// <summary>
        /// Get Open events
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="timeOffset">time offset to utc</param>
        /// <param name="pageNo">page number for pagination</param>
        /// <param name="limit">limit for a each page for pagination</param>
        /// <returns>public sprint participants</returns>
        Task<List<PublicSprintWithParticipantsDto>> GetOpenEvents(int? status, int userId, int timeOffset, int pageNo, int limit);

        /// <summary>
        /// Duplicate a sprint, TODO : remove user object passing
        /// </summary>
        /// <param name="user">user who creating the sprint</param>
        /// <param name="name"> name for sprint</param>
        /// <param name="distance"> distance in meters for sprint</param>
        /// <param name="startTime"> start time for sprint</param>
        /// <param name="type"><see cref="SprintType">sprint type, public or private</see></param>
        /// <param name="numberOfParticipants">number of pariticipant for sprint</param>
        /// <param name="infulenceEmail">infulence email</param>
        /// <param name="draft">sprint draft or publish</param>
        /// <param name="influencerAvailability">influencer available or not</param>
        /// <returns>cereated sprint</returns>
        Task<CreateSprintDto> DuplicateSprint(User user, string name, int distance, DateTime startTime, int type, int? numberOfParticipants, string infulenceEmail, int draft, bool influencerAvailability);

        /// <summary>
        /// Get SprintReportDto by timespan
        /// </summary>
        /// <param name="timespan">timespanc of the report</param>
        /// <returns>SprintReportDto</returns>
        Task<List<ReportItemDto>> GetReport(string timespan);

        /// <summary>
        /// Get All User Mails
        /// </summary>
        /// <returns></returns>
        Task<List<UserMailReportDto>> GetAllUserMails();

        /// <summary>
        /// Validate Private Sprint Count For User
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="lapsTime"> laps Time </param>
        /// <param name="privateSprintCount"> Limit of Private sprints </param>
        /// <returns></returns>
        Task<bool> ValidatePrivateSprintCountForUser(int userId, int lapsTime, int privateSprintCount);
        /// <summary>
        /// Update Sprint Status By SprintId
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        bool UpdateSprintStatusBySprintId(int sprintId);
        /// <summary>
        /// Get All Image Urls
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetAllImages();

        /// <summary>
        /// Get Sprint Paticipants Counts
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        Task<int> GetSprintPaticipantsCounts(int sprintId);

        /// <summary>
        /// Create New Sprint Program
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sprintProgramDto"></param>
        /// <returns></returns>
        Task<SprintProgramDto> CreateNewSprintProgram(User user, SprintProgramDto sprintProgramDto);

        /// <summary>
        /// GetSprintProgramDetailsByUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sprintProgramId"></param>
        /// <returns></returns>
        Task<SprintProgramDto> GetSprintProgramDetailsByUser(int userId, int sprintProgramId);

        /// <summary>
        /// Update Sprint Program
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sprintProgramDto"></param>
        /// <returns></returns>
        Task<SprintProgramDto> UpdateSprintProgram(User user, SprintProgramDto sprintProgramDto);

        /// <summary>
        /// Get All Sprint Programms
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<SprintProgram>> GetAllSprintProgramms(int userId, int pageNo, int limit);

        /// <summary>
        /// Get All Sprint Programms Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetAllSprintProgrammsCount(int userId);
    }
}