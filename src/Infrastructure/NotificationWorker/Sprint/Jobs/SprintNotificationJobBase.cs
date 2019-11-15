namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using System;
    using FirebaseAdmin.Messaging;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    ///
    /// </summary>
    public abstract class SprintNotificationJobBase : SprintNotificationPersistence
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public SprintNotificationJobBase(ScrowdDbContext context) : base(context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="notificationType"></param>
        /// <param name="tokens"></param>
        /// <param name="notificationData"></param>
        /// <returns></returns>
        public virtual MulticastMessage BuildNotificationMessage(
            int notificationId,
            int notificationType,
            List<string> tokens,
            dynamic notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = JsonConvert.SerializeObject(notificationData);
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", notificationType.ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", payload);
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification("Sprint Invite Notification", "sprint demo")
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="distance"></param>
        /// <param name="startTime"></param>
        /// <param name="numberOfParticipants"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public NotificationSprintInfo SetSprintInfo(int id, string name, int distance, DateTime startTime, int numberOfParticipants, SprintType type, SprintStatus status)
        {
            return new NotificationSprintInfo(id, name, distance, startTime, numberOfParticipants, type, status);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="profilePicture"></param>
        /// <param name="code"></param>
        /// <param name="colorCode"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public NotificationUserInfo SetSprintPariticipant(int id, string name, string profilePicture, string code, string colorCode, string city, string country, string countryCode)
        {
            return new NotificationUserInfo(id, name, profilePicture, code, colorCode, city, country, countryCode);
        }

        /// <summary>
        ///
        /// </summary>
        public class NotificationSprintInfo
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="distance"></param>
            /// <param name="startTime"></param>
            /// <param name="numberOfParticipants"></param>
            /// <param name="type"></param>
            /// <param name="status"></param>
            public NotificationSprintInfo(int id, string name, int distance, DateTime startTime, int numberOfParticipants, SprintType type, SprintStatus status)
            {
                this.Id = id;
                this.Name = name;
                this.Distance = distance;
                this.StartTime = startTime;
                this.NumberOfParticipants = numberOfParticipants;
                this.SprintStatus = status;
                this.SprintType = type;
            }
            public int Id { get; }
            public string Name { get; }
            public int Distance { get; }
            public DateTime StartTime { get; }
            public int NumberOfParticipants { get; }
            public SprintStatus SprintStatus { get; }
            public SprintType SprintType { get; }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class NotificationUserInfo
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="profilePicture"></param>
        /// <param name="code"></param>
        /// <param name="colorCode"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="countryCode"></param>
        public NotificationUserInfo(int id, string name, string profilePicture, string code, string colorCode, string city, string country, string countryCode)
        {
            this.Id = id;
            this.Name = name;
            this.ProfilePicture = profilePicture ?? string.Empty;
            this.Code = code;
            this.ColorCode = colorCode;
            this.City = city ?? string.Empty;
            this.Country = country ?? string.Empty;
            this.CountryCode = countryCode ?? string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}