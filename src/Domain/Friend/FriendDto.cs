namespace SprintCrowd.BackEnd.Domain.Friend
{
    /// <summary>
    /// Friend details
    /// </summary>
    public class FriendDto
    {
        /// <summary>
        /// Initialize <see cref="FriendDto">FriendDto</see>
        /// </summary>
        /// <param name="userId">user id of the friend</param>
        /// <param name="name">name for the friend</param>
        /// <param name="profilePicture">profile picture for user</param>
        public FriendDto(int userId, string name, string profilePicture)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
        }

        /// <summary>
        /// Gets friend user ids
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets friend name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets friend profile picutre url
        /// </summary>
        public string ProfilePicture { get; }
    }
}