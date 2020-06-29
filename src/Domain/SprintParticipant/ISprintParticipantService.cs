namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
    using SprintCrowd.BackEnd.Web.SprintManager;

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
        /// <returns><see cref="ExitSprintResult"> Exist sprint result</see></returns>
        Task<ExitSprintResult> ExitSprint(int sprintId, int userId);

        /// <summary>
        /// Get all pariticipant with given stage <see cref="ParticipantStage"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="ParticipantInfoDto"> list of participant info</see></returns>
        Task<List<ParticipantInfoDto>> GetParticipants(int sprintId, ParticipantStage stage);

        /// <summary>
        /// Get all pariticipant with given sprint <see cref="ParticipantStage"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <returns><see cref="ParticipantInfoDto"> list of participant info</see></returns>
        Task<List<ParticipantMinInfoDto>> GetAllParticipants(int sprintId);

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
        Task<GetSprintDto> GetSprints(int userId, SprintType? sprintType, ParticipantStage? stage, int? distanceFrom, int? distanceTo, int? startFrom, int? currentTimeBuff);

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
        List<dynamic> GetNotification(int userId);

        /// <summary>
        /// Remove sprint participant form  sprint
        /// </summary>
        /// <param name="requesterId">requester user id</param>
        /// <param name="sprintId">sprint id</param>
        /// <param name="participantId">participant id for remove</param>
        Task RemoveParticipant(int requesterId, int sprintId, int participantId);

        /// <summary>
        /// Remove participant from  simulation
        /// </summary>
        /// <param name="sprintId">sprint id</param>
        /// <param name="participantId">participant id for remove</param>
        Task RemoveSimulationParticipant(int sprintId, int participantId);

        /// <summary>
        /// Get friend status in sprint
        /// </summary>
        /// <param name="userId">user id </param>
        /// <param name="sprintId">sprint id</param>
        /// <returns><see cref="FriendInSprintDto">friend in sprint </see></returns>
        List<FriendInSprintDto> GetFriendsStatusInSprint(int userId, int sprintId);

        /// <summary>
        /// Remove notification
        /// </summary>
        /// <param name="notificationId">notificaiton id to remove</param>
        Task RemoveNotification(int notificationId);

        /// <summary>
        /// Get sprint details with who marked attendance with given user id
        /// </summary>
        /// <param name="userId">user id to get record</param>
        /// <returns><see cref="SprintInfo">class </see></returns>
        Task<SprintInfo> GetSprintWhichMarkedAttendance(int userId);

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
        Task UpdateParticipantStatus(int userId, int sprintId, DateTime time, ParticipantStage stage);

        Task<SprintInfo> GetSprint(int sprintId);

        Task SprintExpired(int sprintId, List<NotCompletedRunners> notCompletedRunners);

        /// <summary>
        /// Add users to a simulation
        /// </summary>
        /// <param name="sprintId">sprint id going to join</param>
        /// <param name="userIds">user id who going to join</param>
        Task JoinSimulation(List<int> userIds, int sprintId);
    }
}