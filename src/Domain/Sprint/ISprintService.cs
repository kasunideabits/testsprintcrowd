namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Event;

    /// <summary>
    /// ISprintService interface
    /// </summary>
    public interface ISprintService
    {
        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>Available events</returns>
        Task<List<Sprint>> GetAll(int eventType);

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
        /// <returns>cereated sprint</returns>
        Task<CreateSprintDto> CreateNewSprint(User user, string name, int distance, DateTime startTime, int type, int? numberOfParticipants, string infulenceEmail, int draft);

        Task<SprintWithPariticpantsDto> GetSprintByCreator(int userId);

        /// <summary>
        /// update sprint
        Task<UpdateSprintDto> UpdateSprint(int sprintId, string name, int? distance, DateTime? startTime, int? numberOfParticipants, string influencerEmail, int? draftEvent);

        /// <summary>
        /// Get the sprint details and sprint participant details with given
        /// sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <returns><see cref="SprintWithPariticpants">sprint details</see></returns>
        Task<SprintWithPariticpantsDto> GetSprintWithPaticipants(int sprintId);
    }
}