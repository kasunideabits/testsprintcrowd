namespace SprintCrowd.BackEnd.Domain.Friend
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// Interface for handle friend
  /// </summary>
  public interface IFriendRepo
  {
    /// <summary>
    /// Generate friend request code and store
    /// </summary>
    /// <param name="userId">user id for who send the request</param>
    /// <param name="code">uniqe code for request</param>
    Task GenerateFriendCode(int userId, string code);

    /// <summary>
    /// Add given user with matching friend code
    /// </summary>
    /// <param name="userCode">senders unique id</param>
    /// <param name="friendId">reponders user id</param>
    /// <param name="friendCode">generate friend code</param>
    Task AddFriend(string userCode, int friendId, string friendCode);

    /// <summary>
    /// Get firends for given user id
    /// </summary>
    /// <param name="userId">user id to lookup</param>
    /// <returns><see cref="User">list of users</see></returns>
    // Task<List<User>> GetFriends(int userId);

    /// <summary>
    /// Remove friend from user list
    /// </summary>
    /// <param name="userId">user id for requester</param>
    /// <param name="frindId">user id of friend</param>
    Task RemoveFriend(int userId, int frindId);

    /// <summary>
    /// commit and save changes to the db
    /// only call this from the service, DO NOT CALL FROM REPO ITSELF
    /// Unit of work methology.
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Get User with given friend code
    /// </summary>
    /// <param name="friendCode">generate friend code</param>
    /// <returns><see cref="User">user details</see></returns>
    Task<User> GetUserWithCode(string friendCode);

    /// <summary>
    /// Add friend relation
    /// </summary>
    /// <param name="friend">senders unique id</param>
    Task<Friend> PlusFriend(Friend friend);

    /// <summary>
    /// Check whether friendship exist
    /// </summary>
    /// <param name="acceptedId">accepted user id</param>
    /// <param name="sharedId">shared user id</param>
    Task<Friend> checkFiendShip(int acceptedId, int sharedId);

    /// <summary>
    /// Add friend relation
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    Task<List<Friend>> GetAllFriends(int userId);
  }
}