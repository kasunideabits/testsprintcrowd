namespace SprintCrowd.BackEnd.Domain.Friend
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Friend data for lookup
    /// </summary>
    public class FriendData
    {
        /// <summary>
        ///  Initialize <see cref="FriendData">class</see>
        /// </summary>
        /// <param name="requestId">requset id</param>
        /// <param name="user">user object</param>
        public FriendData(int requestId, User user)
        {
            this.RequestId = requestId;
            this.User = user;
        }

        /// <summary>
        /// Gets request id
        /// </summary>
        public int RequestId { get; }

        /// <summary>
        /// Gets user details
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Gets request status
        /// </summary>
    }
}