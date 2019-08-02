namespace SprintCrowd.BackEnd.Web.Friend
{
    /// <summary>
    /// Add friend request representation
    /// </summary>
    public class AddFriendModel
    {
        /// <summary>
        /// Gets or set user id who send the request
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  Gets or set user id for whom to send
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        ///  Gets or set unique code for friend request
        /// </summary>
        public int Code { get; set; }
    }
}