namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    using System;
    using src.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowdBackEnd.Infrastructure.NotificationWorker.Sprint.Models;

    /// <summary>
    /// Available sprint notificaitons
    /// </summary>
    public class SprintNotificationJobs : ISprintNotificationJobs
    {
        /// <summary>
        /// Sprint invite notifications
        /// </summary>
        public void SprintInvite(int sprintId, int iniviteId, int inviteeId)
        {
            var message = new InviteSprint(sprintId, iniviteId, inviteeId);
            new NotificationWorker<SprintInvite>().Invoke(message);
        }

        /// <summary>
        /// Sprint mark attendance
        /// </summary>
        public void SprintMarkAttendace(int sprintId, int userId, string name, string profilePicture, string country, string countryCode, string city, string colorCode )
        {
            var message = new MarkAttendance(sprintId, userId, name, profilePicture, country, countryCode, city, colorCode);
            new NotificationWorker<SprintMarkAttendance>().Invoke(message);
        }

        /// <summary>
        /// Sprint exit
        /// </summary>
        public void SprintExit(
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
            string countryCode)
        {
            var message = new ExitSprint(
                sprintId, sprintName, distance, startTime, numberOfParticipant, sprintStatus, sprintType, creatorId,
                userId, userStage, name, profilePicture, code, city, country, countryCode);
            new NotificationWorker<SprintExit>().Invoke(message);
        }

        /// <summary>
        /// Sprint join
        /// </summary>
        public void SprintJoin(int sprintId, string sprintName, SprintType sprintType, int userId, string name, string profilePicture, bool accept)
        {
            var message = new JoinSprint(sprintId, sprintName, sprintType, userId, name, profilePicture, accept);
            new NotificationWorker<SprintJoin>().Invoke(message);
        }

        public void SprintRemove(
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
        )
        {
            var message = new RemoveSprint(
                sprintId,
                sprintName,
                distance,
                startTime,
                numberOfParticipant,
                sprintStatus,
                sprintType,
                userId, name,
                profilePicture,
                code,
                colorCode,
                city,
                country,
                countryCode);
            new NotificationWorker<SprintRemove>().Invoke(message);
        }

        public void SprintUpdate(int sprintId, string oldSprintName, string newSprintName, int distance, DateTime startTime, int numberOfParticipant, SprintStatus sprintStatus, SprintType sprintType, int creatorId)
        {
            var message = new UpdateSprint(sprintId, oldSprintName, newSprintName, distance, startTime, numberOfParticipant, sprintStatus, sprintType, creatorId);
            new NotificationWorker<SprintUpdate>().Invoke(message);
        }

        public void AcceptRequest(int id, string name, string profilePicture, string code, string email, string city, string country, string countryCode, string colorCode, DateTime createdDate ,int requestSenderId)
        {
            var message = new AcceptRequest(id, name, profilePicture, code, email, city, country, countryCode, colorCode,  createdDate , requestSenderId);
            new NotificationWorker<UserAcceptRequest>().Invoke(message);
        }

        public void InviteRequest(int id, string name, string profilePicture, DateTime createdDate, int requestSenderId,string text)
        {
            var message = new InviteFriend(id,  profilePicture, name,  createdDate, requestSenderId, text);
            new NotificationWorker<UserInviteRequest>().Invoke(message);
        }

        public void DeclineRequest(int id, string name, string profilePicture, DateTime createdDate, int requestSenderId, string text)
        {
            var message = new DeclineFriend(id, profilePicture, name, createdDate, requestSenderId, text);
            new NotificationWorker<UserDeclinedRequest>().Invoke(message);
        }


        public void SprintParticipantRemove(int sprintId, SprintType sprintType, SprintStatus sprintStatus, int creatorId, int userId,
                                string creatorName, string sprintName, DateTime startTime, int numOfparticipant, int distance,
                                string name, string profilePicture, string code, string country, string countryCode, string city)
        {
            var message = new RemoveParticipant(sprintId, sprintType, sprintStatus, creatorId, userId, creatorName, sprintName, startTime, numOfparticipant, distance, name, profilePicture, code, country, countryCode, city);
            new NotificationWorker<SprintParticipantRemove>().Invoke(message);
        }
    }
}