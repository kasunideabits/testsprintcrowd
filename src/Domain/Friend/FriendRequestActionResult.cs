namespace SprintCrowd.BackEnd.Domain.Friend
{
    /// <summary>
    /// Friend request action result
    /// </summary>
    public enum FriendRequestActionResult
    {
        /// Code is not found
        RequestCodeNotFound,

        /// Already added a user with given friend code
        AlreadyUsedCode,

        /// Expired firend code
        ExpiredCode,
    }
}