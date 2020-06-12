namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

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
        /// <returns>Available events</returns>
        Task<List<Sprint>> GetAll(int eventType, string searchTerm, string sortBy, string filterBy);

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
        /// Create a new sprint, TODO : remove user object passing
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
        Task<CreateSprintDto> CreateNewSprint(User user, string name, int distance, DateTime startTime, int type, int? numberOfParticipants, string infulenceEmail, int draft, bool influencerAvailability);

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
        Task CreateMultipleSprints(User user, string name, int distance, DateTime startTime, int type, int? numberOfParticipants, string infulenceEmail, int draft, bool influencerAvailability, string repeatType);

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
        Task<UpdateSprintDto> UpdateSprint(int userId, int sprintId, string name, int? distance, DateTime? startTime, int? numberOfParticipants, string influencerEmail, int? draftEvent);

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
        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        Task<SprintWithPariticpantsDto> GetSprintWithPaticipants(int sprintId);

        Task InviteRequest(int inviterId, int inviteeId, int sprintId);

        /// <summary>
        /// Get public sprint with user preference
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="timeOffset">time offset to utc</param>
        /// <returns>sprint with participant info</returns>
        Task<List<PublicSprintWithParticipantsDto>> GetPublicSprints(int userId, int timeOffset);

        Task<List<PublicSprintWithParticipantsDto>> GetOpenEvents(int userId, int timeOffset);

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
        Task<List<SprintReportDto>> GetReport(string timespan);

    }
}