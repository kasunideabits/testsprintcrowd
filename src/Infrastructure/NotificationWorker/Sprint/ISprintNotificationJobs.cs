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
            int userId,
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

        void SprintUpdate(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipant, SprintStatus sprintStatus, SprintType sprintType, int creatorId);
    }
}