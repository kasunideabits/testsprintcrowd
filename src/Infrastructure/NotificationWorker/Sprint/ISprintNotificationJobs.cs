using System;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    /// <summary>
    /// Sprint notification types interface
    /// </summary>
    public interface ISprintNotificationJobs
    {
        /// <summary>
        /// Sprint invite
        /// </summary>
        void SprintInvite(int sprintId, int iniviteId, int inviteeId);

        /// <summary>
        /// Sprint mark attendance
        /// </summary>
        void SprintMarkAttendace(int sprintId, int userId, string name, string profilePicture, string country, string countryCode, string city, string colorCode);

        /// <summary>
        /// Sprint exit
        /// </summary>
        void SprintExit(
            int sprintId,
            string sprintName,
            int distance,
            DateTime startTime,
            int numberOfParticipant,
            SprintStatus sprintStatus,
            SprintType sprintType,
            int creatorId,
            int userId,
            int userStage,
            string name,
            string profilePicture,
            string code,
            string city,
            string country,
            string countryCode);

        /// <summary>
        /// Sprint join
        /// </summary>
        void SprintJoin(int sprintId, string sprintName, SprintType sprintType, int userId, string name, string profilePicture, bool accepet);

        void SprintRemove(
            int sprintId,
            string sprintName,
            int distance,
            DateTime startTime,
            int numberOfParticipant,
            SprintStatus sprintStatus,
            SprintType sprintType,
            int userId,
            string name,
            string profilePicture,
            string code,
            string colorCode,
            string city,
            string country,
            string countryCode
        );

        void SprintUpdate(int sprintId, string oldSprintName, string newSprintName, int distance, DateTime startTime, int numberOfParticipant, SprintStatus sprintStatus, SprintType sprintType, int creatorId);
        void AcceptRequest(int id, string name, string profilePicture, string code, string email, string city, string country, string countryCode, string colorCode, DateTime createdDate, int requestSenderId);

        void DeclineRequest(int id, string name, string profilePicture, DateTime createdDate, int requestSenderId, string text);

        void SprintParticipantRemove(int sprintId, SprintType sprintType, SprintStatus sprintStatus, int creatorId, int userId,
                                string creatorName, string sprintName, DateTime startTime, int numOfparticipant, int distance,
                                string name, string profilePicture, string code, string country, string countryCode, string city);

        void InviteRequest(int id, string name, string profilePicture, DateTime createdDate, int requestSenderId, string text);
    }
}