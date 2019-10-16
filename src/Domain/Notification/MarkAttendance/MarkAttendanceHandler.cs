﻿namespace SprintCrowd.BackEnd.Domain.Notification.MarkAttendance
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Notifier;

    /// <summary>
    /// Class responsible for send notification which indicating x user mark
    /// attendance for y event
    /// </summary>
    public class MarkAttendanceHandler : IMarkAttendanceHandler
    {
        /// <summary>
        /// Initialize MarkAttendanceHandler class
        /// </summary>
        /// <param name="notifyFactory">notification factory</param>
        public MarkAttendanceHandler(INotifyFactory notifyFactory)
        {
            this.NotifyFactory = notifyFactory;
        }

        private INotifyFactory NotifyFactory { get; }

        /// <summary>
        /// Execute notification task
        /// </summary>
        /// <param name="markAttendance">mark attendance paritiipant details</param>
        /// <returns>task completed or not</returns>
        public Task Execute(MarkAttendance markAttendance)
        {
            var message = new MarkAttendanceNotification(
                markAttendance.SprintId,
                markAttendance.UserId,
                markAttendance.Name,
                markAttendance.ProfilePicture == null ? string.Empty : markAttendance.ProfilePicture,
                markAttendance.Country == null ? string.Empty : markAttendance.Country,
                markAttendance.CountryCode == null ? string.Empty : markAttendance.CountryCode,
                markAttendance.City == null ? string.Empty : markAttendance.City,
                markAttendance.ColorCode == null ? new UserColorCode().PickColor() : markAttendance.ColorCode);
            this.SendNotification(markAttendance.SprintId, message);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Send notification message via <see cref="NotifyFactory"/>
        /// </summary>
        /// <param name="sprintId">sprint id for marked attendance</param>
        /// <param name="message"><see cref="MarkAttendanceNotification"> notification message </see></param>
        private Task SendNotification(int sprintId, MarkAttendanceNotification message)
        {
            IChannel channel = this.NotifyFactory.CreateChannel(ChannelNames.GetChannel(sprintId));
            channel.Publish(EventNames.GetEvent(), message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Mark attendance event notification message, define who was marked the attendance
        /// </summary>
        internal class MarkAttendanceNotification
        {
            /// <summary>
            /// Initialize <see cref="MarkAttendanceNotification"/> class
            /// </summary>
            /// <param name="sprintId">sprint id</param>
            /// <param name="userId">user id</param>
            /// <param name="name">name for user</param>
            /// <param name="profilePicture">url for user's profile picture</param>
            /// <param name="country">country for user</param>
            /// <param name="countryCode">country code for user</param>
            /// <param name="city">city for user</param>
            /// <param name="colorCode">color code for user</param>
            public MarkAttendanceNotification(
                int sprintId,
                int userId,
                string name,
                string profilePicture,
                string country,
                string countryCode,
                string city,
                string colorCode)
            {
                this.SprintId = sprintId;
                this.UserId = userId;
                this.Name = name;
                this.ProfilePicture = profilePicture;
                this.Country = country;
                this.CountryCode = countryCode;
                this.City = city;
                this.ColorCode = colorCode;
            }

            /// <summary>
            /// Gets marked sprint id
            /// </summary>
            public int SprintId { get; }

            /// <summary>
            /// Gets marked user id
            /// </summary>
            public int UserId { get; }

            /// <summary>
            /// Gets name for user
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets uri for user profile picture
            /// </summary>
            public string ProfilePicture { get; }

            /// <summary>
            /// Gets country
            /// </summary>
            public string Country { get; }

            /// <summary>
            /// Gets country code
            /// </summary>
            public string CountryCode { get; }

            /// <summary>
            /// Gets  city
            /// </summary>
            public string City { get; }

            /// <summary>
            /// Gets color code
            /// </summary>
            public string ColorCode { get; }
        }
    }
}