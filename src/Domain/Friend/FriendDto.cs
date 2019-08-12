using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

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
        /// <param name="requestId">request id</param>
        /// <param name="userId">user id of the friend</param>
        /// <param name="name">name for the friend</param>
        /// <param name="profilePicture">profile picture for user</param>
        /// <param name="userCode">frined request code</param>
        public FriendDto(
            int userId,
            string name,
            string profilePicture,
            string userCode)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = userCode;
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

        /// <summary>
        /// Gets user code
        /// </summary>
        public string Code { get; }
    }
}