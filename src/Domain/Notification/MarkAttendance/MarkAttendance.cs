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
        public MarkAttendance(int sprintId, int userId, string name, string profilePicture)
        {
            this.SprintId = sprintId;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
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
    }
}