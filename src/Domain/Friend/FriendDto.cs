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
        /// <param name="status">friend request status</param>
        public FriendDto(
            int requestId,
            int userId,
            string name,
            string profilePicture,
            string userCode,
            FriendRequestStatus status)
        {
            this.RequestId = requestId;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = userCode;
            this.Status = status;
        }

        /// <summary>
        /// Gets request id
        /// </summary>
        public int RequestId { get; }

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

        /// <summary>
        /// Friend request status
        /// </summary>
        public FriendRequestStatus Status { get; }
    }
}