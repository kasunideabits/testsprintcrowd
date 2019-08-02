namespace SprintCrowd.BackEnd.Web.Friend
{
    /// <summary>
    /// Action request body for friend request
    /// </summary>
    public class FriendRequestActionModel
    {
        /// <summary>
        /// Gets or set request id
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Gets or set user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or set friend id
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Gets or set unique code
        /// </summary>
        public int Code { get; set; }
    }
}