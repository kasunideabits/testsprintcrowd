using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    public class JoinSprint
    {
        /// <summary>
        /// Initialize <see cref="JoinSprint"/> class
        /// </summary>
        /// <param name="sprintId">sprint id which joined</param>
        /// <param name="sprintName">sprint name which joined</param>
        /// <param name="userId">user id for who has joined</param>
        /// <param name="name">name for who has joined</param>
        /// <param name="profilePicture">profile picture url for who has joined</param>
        public JoinSprint(int sprintId, string sprintName, SprintType sprintType, int userId, string name, string profilePicture)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.SprintType = sprintType;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
        }

        /// <summary>
        /// Gets sprint id
        /// </summary>
        public int SprintId { get; }

        /// <summary>
        /// Gets sprint name
        /// </summary>
        public string SprintName { get; }

        public SprintType SprintType { get; }

        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets user's name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets user's profile picture url
        /// </summary>
        public string ProfilePicture { get; }
    }
}