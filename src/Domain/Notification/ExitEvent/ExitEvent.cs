namespace SprintCrowd.BackEnd.Domain.Notification.ExitEvent
{
    /// <summary>
    /// class which give information for exit event notification
    /// </summary>
    public class ExitEvent
    {
        /// <summary>
        /// Initialize <see cref="ExitEvent"/> class
        /// </summary>
        /// <param name="sprintId">sprint id which exit</param>
        /// <param name="sprintName">sprint name which exit</param>
        /// <param name="userId">user id for who has exit</param>
        /// <param name="name">name for who has exit</param>
        /// <param name="profilePicture">profile picture url for who has exit</param>
        public ExitEvent(
            int sprintId,
            string sprintName,
            int userId,
            string name,
            string profilePicture)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
        }

        /// <summary>
        /// Gets sprint id
        /// </summary>
        public int SprintId { get; private set; }

        /// <summary>
        /// Gets sprint name
        /// </summary>
        public string SprintName { get; private set; }

        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gets user's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets user's profile picture url
        /// </summary>
        public string ProfilePicture { get; private set; }
    }
}