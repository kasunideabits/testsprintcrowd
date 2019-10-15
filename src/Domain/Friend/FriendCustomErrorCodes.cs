namespace SprintCrowd.BackEnd.Application
{
  /// <summary>
  /// response codes to use when responding to rest requests.
  /// </summary>
  public enum FriendCustomErrorCodes
  {
    /// <summary>
    /// Invalid user code.
    /// </summary>
    InvalidUserCode = 1000,

    /// <summary>
    /// Already friends.
    /// </summary>
    AlreadyFriends = 1200,

    /// <summary>
    /// Not friends.
    /// </summary>
    InvalidUserId = 1230
  }
}