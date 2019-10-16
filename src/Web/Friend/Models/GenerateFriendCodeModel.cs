namespace SprintCrowd.BackEnd.Web.Friend
{
    /// <summary>
    /// Add friend request representation
    /// </summary>
    public class GenerateFriendCodeModel
    {
        /// <summary>
        /// Gets or set user id who send the request
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  Gets or set unique code for user
        /// </summary>
        public string Code { get; set; }
    }
}