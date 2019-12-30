namespace SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Sprint reminder message instance
    /// </summary>
    internal class SprintReminderMessage
    {
        /// <summary>
        ///  Initialize SprintReminderMessage class
        /// </summary>
        /// <param name="id">sprint id for scheule task </param>
        /// <param name="name">sprint name</param>
        /// <param name="distance"></param>
        /// <param name="startTime"></param>
        /// <param name="numberOfPariticipants"></param>
        /// <param name="sprintType"></param>
        /// <param name="sprintStatus"></param>
        /// <param name="notificationType">notification reminder time indication</param>
        public SprintReminderMessage(int id, string name, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus, SprintNotificaitonType notificationType)
        {
            this.Sprint = new SprintInfo(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus);
            this.NotificationType = notificationType;
        }

        public SprintInfo Sprint { get; set; }

        /// <summary>
        /// notification reminder time indication
        /// </summary>
        public SprintNotificaitonType NotificationType { get; }
    }

    internal class SprintInfo
    {
        public SprintInfo(int id, string name, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.NumberOfPariticipants = numberOfPariticipants;
            this.SprintType = sprintType;
            this.SprintStatus = sprintStatus;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public DateTime StartTime { get; set; }
        public int NumberOfPariticipants { get; set; }
        public SprintType SprintType { get; set; }
        public SprintStatus SprintStatus { get; set; }
    }
}