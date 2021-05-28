namespace src.Infrastructure.NotificationWorker.Sprint.Models
{
    public class MarkAttendance
    {
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