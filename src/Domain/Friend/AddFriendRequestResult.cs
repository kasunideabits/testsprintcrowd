namespace SprintCrowd.BackEnd.Domain.Friend
{
    /// <summary>
    /// Class define add friend requst result
    /// </summary>
    public static class AddFriendRequestResult
    {

        /// <summary>
        /// Add friend request success
        /// </summary>
        public static string Success() => "Successfuly generate friend request";

        /// <summary>
        ///  Add friend request faild
        /// </summary>
        public static string Faild() => "Friend request generate faild";
    }
}