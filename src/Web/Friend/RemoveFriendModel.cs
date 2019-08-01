namespace SprintCrowd.BackEnd.Web.Friend
{
    /// <summary>
    /// Remove friend request representation
    /// </summary>
    public class RemoveFriendModel
    {
        /// <summary>
        /// Gets or set user id who send the request
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  Gets or set user id for whom to remove
        /// </summary>
        public int FriendId { get; set; }
    }
}