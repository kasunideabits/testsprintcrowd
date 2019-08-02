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
        /// <param name="user">user object</param>
        /// <param name="status">status for request</param>
        public FriendData(User user, FriendRequestStatus status)
        {
            this.User = user;
            this.Status = status;
        }

        /// <summary>
        /// Gets user details
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Gets request status
        /// </summary>
        public FriendRequestStatus Status { get; }
    }
}