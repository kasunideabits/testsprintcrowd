namespace SprintCrowd.BackEnd.Web.Friend
{
    /// <summary>
    /// Remove friend error results
    /// </summary>
    public static class RemoveFriendResult
    {
        /// <summary>
        /// User id can't be changed
        /// </summary>
        public static object NullUserId()
        {
            return new
            {
                Result = (int)RemoveFriendErrorCode.NullUserId,
                    Reason = RemoveFriendErrorReason.NullUserId,
            };
        }

        /// <summary>
        /// Friend id can't be null
        /// </summary>
        public static object NullFriendId()
        {
            return new
            {
                Result = (int)RemoveFriendErrorCode.NullFriendId,
                    Reason = RemoveFriendErrorReason.NullFriendId
            };
        }

        /// <summary>
        /// Error code for remove friends
        /// </summary>
        protected enum RemoveFriendErrorCode
        {
            /// <summary>
            /// User id can't be null
            /// </summary>
            NullUserId,

            /// <summary>
            /// Friend id can't be null
            /// </summary>
            NullFriendId
        }

        /// <summary>
        /// Remove friend error code reasons
        /// </summary>
        protected class RemoveFriendErrorReason
        {
            /// <summary>
            /// User id can't be null
            /// </summary>
            public const string NullUserId = "User id can't be null";

            /// <summary>
            /// Friend id can't be null
            /// </summary>
            public const string NullFriendId = "Friend id can't be null";
        }

    }

}