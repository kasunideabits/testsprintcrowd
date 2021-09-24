namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
    using SprintCrowd.BackEnd.Web.SprintManager;
    using SprintCrowdBackEnd.Web.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Domain.SprintParticipant.Dtos;
    using SprintCrowdBackEnd.Domain.Sprint.Dtos;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Interface for sprint participant service
    /// </summary>
    public interface ISprintParticipantService
    {
        /// <summary>
        /// Mark the attendece for the given sprint and notify with evnet
        /// EventName.MarkedAttenence with MarkAttendenceMessage message
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for for participant</param>
        Task MarkAttendence(int sprintId, int userId);

        /// <summary>
        /// Join user for a sprint
        /// </summary>
        /// <param name="sprintId">sprint id going to join</param>
        /// <param name="userId">user id who going to join</param>
        /// <param name="notificationId"> notification id</param>
        /// <param name="accept">accept or decline</param>
        Task<ParticipantInfoDto> JoinSprint(int sprintId, int userId, int notificationId, bool accept);

        /// <summary>
        /// Exit sprint which join for event
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        /// <param name="distance">user running distance</param>
        /// <param name="raceCompletedDuation">user runner race completed duration</param>
        /// <returns><see cref="ExitSprintResult"> Exist sprint result</see></returns>
        Task<ExitSprintResult> ExitSprint(int sprintId, int userId, int distance, string raceCompletedDuation);

        /// <summary>
        /// Get all pariticipant with given stage <see cref="ParticipantStage"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="ParticipantInfoDto"> list of participant info</see></returns>
        Task<List<ParticipantInfoDto>> GetParticipants(int sprintId, ParticipantStage stage);

        /// <summary>
        /// Get all sprint info with given filters
        /// Change request 12/11/2019 Mobile application event start now tab require user already created event
        /// reguradless 24H, for easyness change this API to send creator event embedded with sprints
        /// @todo remove this change and handle this in mobile side
        /// </summary>
        /// <param name="userId">participant id</param>
        /// <param name="sprintType"><see cref="SprintType"> sprint type</see></param>
        /// <param name="stage"><see cref="ParticipantStage"> participant stage</see></param>
        /// <param name="distanceFrom">distance in meters from</param>
        /// <param name="distanceTo">distance in meters from</param>
        /// <param name="startFrom">start from time in minutes</param>
        /// <param name="currentTimeBuff">current time difference</param>
        /// <returns><see cref="SprintInfo"> sprint info </see> </returns>
        Task<List<GetCommonSprintDto>> GetSprints(int userId, SprintType? sprintType, ParticipantStage? stage, int? distanceFrom, int? distanceTo, int? startFrom, int? currentTimeBuff);

        /// <summary>
        /// Invite user to sprint
        /// </summary>
        /// <param name="sprintId">sprint id</param>
        /// <param name="inviterId">id of inviter</param>
        /// <param name="inviteeIds">ids for invitess</param>
        /// <returns>invited users info</returns>
        Task<List<ParticipantInfoDto>> SprintInvite(int sprintId, int inviterId, List<int> inviteeIds);

        /// <summary>
        /// Get all notificaitons
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>all notificaiton related to given userid</returns>
        Notifications GetNotification(int userId);
       
        /// <summary>
        /// Remove sprint participant form  sprint
        /// </summary>
        /// <param name="requesterId">requester user id</param>
        /// <param name="sprintId">sprint id</param>
        /// <param name="participantId">participant id for remove</param>
        Task RemoveParticipant(int requesterId, int sprintId, int participantId);

        /// <summary>
        /// Get friend status in sprint
        /// </summary>
        /// <param name="userId">user id </param>
        /// <param name="sprintId">sprint id</param>
        /// <returns><see cref="FriendInSprintDto">friend in sprint </see></returns>
        List<FriendInSprintDto> GetFriendsStatusInSprint(int userId, int sprintId);

        /// <summary>
        /// Update User Country Detail By UserId
        /// </summary>
        /// <param name="userCountryInfo"></param>
        /// <returns></returns>
        bool UpdateUserCountryDetailByUserId(UserCountryDetail userCountryInfo);

        /// <summary>
        /// Remove notification
        /// </summary>
        /// <param name="notificationId">notificaiton id to remove</param>
        Task RemoveNotification(int notificationId);

       
        /// <summary>
        /// Get statistics for given user id
        /// </summary>
        /// <param name="userId"> user id to fetch</param>
        /// <returns>get all statistics for public and private sprints </returns>
        SprintStatisticDto GetStatistic(int userId);

        /// <summary>
        /// Get all joined sprints for given date
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <param name="fetchDate">fetch date</param>
        /// <returns>joined sprints</returns>
        JoinedSprintsDto GetJoinedEvents(int userId, DateTime fetchDate);

        /// <summary>
        /// Update sprint completed or not and time
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sprintId"></param>
        /// <param name="time"></param>
        /// <param name="stage"></param>
        Task UpdateParticipantStatus(int userId, int sprintId, DateTime time, ParticipantStage stage, double position, string raceCompletedDuration, double distance);

        Task<SprintInfo> GetSprint(int sprintId);

        Task SprintExpired(int sprintId, List<NotCompletedRunners> notCompletedRunners);

        /// <summary>
        /// Get All Sprints History By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Sprint>> GetAllSprintsHistoryByUserId(int userId, int pageNo, int limit);

        /// <summary>
        /// Get All Sprints History Count By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetAllSprintsHistoryCountByUserId(int userId);
       
        /// <summary>
        /// Get sprint parcipant details (starttime,finshtime)
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SprintParticipantDto> GetSprintParticipant(int sprintId, int userId);

        /// <summary>
        /// Get sprint details with who marked attendance with given user id
        /// </summary>
        /// <param name="userId">user id to get record</param>
        /// <returns><see cref="SprintInfo">class </see></returns>
        Task<SprintInfo> GetSprintWhichMarkedAttendance(int userId);

        /// <summary>
        /// Get sprint details with participant's profiles.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<SprintWithParticipantProfile> GetSprintWithParticipantProfile(int userId);
        /// <summary>
        /// Is Allow User To Create Sprints
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsAllowUserToCreateSprints(int userId);

        /// <summary>
        /// Add Sprint Participant Members
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sprintId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task<SprintParticipantMembers> AddSprintParticipantMembers(int userId, int sprintId, string memberId);
    }
}