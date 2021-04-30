namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    /// <summary>
    /// User info data transfer object
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Initalize <see cref="UserDto"> class </see>
        /// </summary>
        /// <param name="userId">unique id for the user</param>
        /// <param name="name">name for user</param>
        /// <param name="profilePicture">profile picture url for user</param>
        /// <param name="userCode">profile picture url for user</param>
        public UserDto(int userId, string name, string profilePicture, string userCode)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = userCode;
        }

        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets user name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets user profile picture
        /// </summary>
        public string ProfilePicture { get; }

        /// <summary>
        /// Get user coed
        /// </summary>
        /// <value></value>
        public string Code { get; }
    }
}