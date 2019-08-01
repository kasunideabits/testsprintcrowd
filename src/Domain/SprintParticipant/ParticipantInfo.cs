namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// class for indicate sprint participant basic info
    /// </summary>
    public class ParticipantInfo
    {
        /// <summary>
        /// Initialize <see cref="ParticipantInfo">ParticipantInfo class </see>
        /// </summary>
        /// <param name="userId">user id for participant</param>
        /// <param name="userName">name for participant</param>
        /// <param name="profilePicture">profile picture url for participant</param>
        /// <param name="sprintId">sprint id which participate</param>
        /// <param name="sprintName">sprint name which participate</param>
        public ParticipantInfo(
            int userId,
            string userName,
            string profilePicture,
            int sprintId,
            string sprintName)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.ProfilePicture = profilePicture;
            this.SprintId = sprintId;
            this.SprintName = sprintName;
        }

        /// <summary>
        /// Gets user id for participant
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets name for participant
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets profile picture url for participant
        /// </summary>
        public string ProfilePicture { get; }

        /// <summary>
        /// Gets sprint id which participate
        /// </summary>
        public int SprintId { get; }

        /// <summary>
        /// Gets sprint name which participate
        /// </summary>
        public string SprintName { get; }
    }
}