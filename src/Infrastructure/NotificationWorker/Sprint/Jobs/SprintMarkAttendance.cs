namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using src.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    /// <summary>
    /// Mark attendance notification handling
    /// </summary>
    public class SprintMarkAttendance : ISprintMarkAttendance
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="ablyFactory">ably connection factory</param>
        public SprintMarkAttendance(IAblyConnectionFactory ablyFactory)
        {
            this.AblyConnectionFactory = ablyFactory;
        }

        private IAblyConnectionFactory AblyConnectionFactory { get; }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"><see cref="MarkAttendance"> mark attendance data </see></param>
        public void Run(object message = null)
        {
            MarkAttendance markAttendance = message as MarkAttendance;
            if (markAttendance != null)
            {
                this.AblyMessage(markAttendance);
                this.SendPushNotification(markAttendance);
            }
        }

        private void AblyMessage(MarkAttendance markAttendance)
        {
            var ablyNotificationMsg = NotificationMessageMapper(markAttendance);
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + markAttendance.SprintId);
            channel.Publish("MarkedAttendece", ablyNotificationMsg);
        }

        private void SendPushNotification(MarkAttendance markAttendance) { System.Console.WriteLine(markAttendance.City); }

        private static MarkAttandanceAblyMesage NotificationMessageMapper(MarkAttendance markAttendance)
        {
            return new MarkAttandanceAblyMesage(
                markAttendance.SprintId,
                markAttendance.UserId,
                markAttendance.Name,
                markAttendance.ProfilePicture == null ? string.Empty : markAttendance.ProfilePicture,
                markAttendance.Country == null ? string.Empty : markAttendance.Country,
                markAttendance.CountryCode == null ? string.Empty : markAttendance.CountryCode,
                markAttendance.City == null ? string.Empty : markAttendance.City,
                markAttendance.ColorCode == null ? new UserColorCode().PickColor() : markAttendance.ColorCode);
        }
    }

    /// <summary>
    /// Mark attendance event notification message, define who was marked the attendance
    /// </summary>
    internal class MarkAttandanceAblyMesage
    {
        /// <summary>
        /// Initialize <see cref="MarkAttandanceAblyMesage "/> class
        /// </summary>
        /// <param name="sprintId ">sprint id</param>
        /// <param name="userId ">user id</param>
        /// <param name="name ">name for user</param>
        /// <param name="profilePicture ">url for user's profile picture</param>
        /// <param name="country ">country for user</param>
        /// <param name="countryCode ">country code for user</param>
        /// <param name="city ">city for user</param>
        /// <param name="colorCode ">color code for user</param>
        public MarkAttandanceAblyMesage(
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