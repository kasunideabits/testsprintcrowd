namespace SprintCrowd.BackEnd.Domain.Friend
{
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using System.Collections.Generic;
  using SprintCrowd.BackEnd.Common;

  /// <summary>
  ///  Interface for sprint crowd frined service
  /// </summary>
  public interface IFriendService
  {

    /// <summary>
    /// Add given user with matching friend code
    /// </summary>
    /// <param name="userId">resonder user id</param>
    /// <param name="friendCode">generate friend code</param>
    /// <returns><see cref="User"></see> and reason</returns>
    Task<AddFriendDTO> PlusFriend(int userId, string friendCode);

    /// <summary>
    /// Get all friends of loggedin user
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    /// <returns><see cref="User"></see> and reason</returns>
    Task<List<FriendListDTO>> AllFriends(int userId);

    /// <summary>
    /// Remove friend from friend list
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    /// <param name="friendId">friend id</param>
    Task<RemoveFriendDTO> DeleteFriend(int userId, int friendId);

    /// <summary>
    /// Get user with user id
    /// </summary>
    /// <param name="userId">user id of the user to be retrieved</param>
    Task<GetFriendDto> GetFriend(int userId);
  }
}