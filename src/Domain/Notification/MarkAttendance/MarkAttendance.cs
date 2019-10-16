namespace SprintCrowd.BackEnd.Domain.Notification.MarkAttendance
{
    /// <summary>
    /// class which give information for makr attendance event notification
    /// </summary>
    public class MarkAttendance
    {
        /// <summary>
        /// Initialize <see cref="MarkAttendance"/> class
        /// </summary>
        /// <param name="sprintId">sprint id for marked attendance</param>
        /// <param name="userId">user who marked attendance</param>
        /// <param name="name">name for user who mark attendance</param>
        /// <param name="profilePicture">url for users profile picture</param>
        /// <param name="country">country for user</param>
        /// <param name="countryCode">country code for user</param>
        /// <param name="city">city for user</param>
        /// <param name="colorCode">color code user</param>
        public MarkAttendance(
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
        /// Gets city
        /// </summary>
        public string City { get; }

        /// <summary>
        /// Gets color code for user
        /// </summary>
        public string ColorCode { get; }
    }
}