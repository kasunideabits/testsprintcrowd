namespace SprintCrowd.BackEnd.Domain.Friend
{
  using SprintCrowd.BackEnd.Application;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using System;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Infrastructure.Persistence;

  /// <summary>
  /// Implement <see cref="IFriendRepo"> repo </see>
  /// </summary>
  public class FriendRepo : IFriendRepo
  {
    private ScrowdDbContext dbContext;
    /// <summary>
    /// Initialize <see cref="FriendRepo"> class </see>
    /// </summary>
    /// <param name="dbContext">database context</param>
    public FriendRepo(ScrowdDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    //private ScrowdDbContext Context { get; }

    /// <summary>
    /// Generate friend request code and store
    /// </summary>
    /// <param name="userId">user id for who send the request</param>
    /// <param name="code">uniqe code for request</param>
    public async Task GenerateFriendCode(int userId, string code)
    {
      Friend friend = new Friend()
      {
        SharedUserId = userId,
        CreatedTime = DateTime.UtcNow,
      };
      await this.dbContext.Frineds.AddAsync(friend);
    }

    /// <summary>
    /// Add given user with matching friend code
    /// </summary>
    /// <param name="userCode">senders unique id</param>
    /// <param name="friendId">reponders user id</param>
    /// <param name="friendCode">generate friend code</param>
    public async Task AddFriend(string userCode, int friendId, string friendCode)
    {
      // var requester = await this.Context.Frineds
      //     .Include(f => f.User)
      //     .FirstOrDefaultAsync(f => f.Code == friendCode && f.User.Code == userCode);
      // if (requester == null)
      // {
      //     throw new Application.ApplicationException(
      //         (int)FriendRequestActionResult.RequestCodeNotFound,
      //         "Invalid request code");
      // }
      // else
      // {
      //     if (requester.FriendId == null)
      //     {
      //         requester.FriendId = friendId;
      //         requester.StatusUpdatedTime = DateTime.UtcNow;
      //         return;
      //     }
      //     else if (requester.FriendId != null)
      //     {
      //         throw new Application.ApplicationException(
      //             (int)FriendRequestActionResult.AlreadyUsedCode,
      //             "Already used code");
      //     }
      // }
    }

    /// <summary>
    /// Get firends for given user id
    /// </summary>
    /// <param name="userId">user id to lookup</param>
    /// <returns><see cref="User">list of users</see></returns>
    // public Task<List<User>> GetFriends(int userId)
    // {
    // var user = await this.Context.User
    //     .Include(f => f.Friends)
    //     .ThenInclude(f => f.User)
    //     .Include(f => f.FriendRequester)
    //     .ThenInclude(f => f.FriendOf)
    //     .FirstOrDefaultAsync(f => f.Id == userId);

    // List<User> friends = new List<User>();
    // if (user != null)
    // {
    //     user.FriendRequester.ForEach(f => friends.Add(f.FriendOf));
    //     user.Friends.ForEach(f => friends.Add(f.User));
    // }
    // return friends;
    // }

    /// <summary>
    /// Remove friend from user list
    /// </summary>
    /// <param name="userId">user id for requester</param>
    /// <param name="friendId">user id of friend</param>
    public async Task RemoveFriend(int userId, int friendId)
    {
      // var friendRel1 = await this.Context.Frineds
      //     .FirstOrDefaultAsync(f =>
      //         (f.UserId == userId && f.FriendId == friendId) ||
      //         (f.UserId == friendId && f.FriendId == userId));
      // if (friendRel1 != null)
      // {
      //     this.Context.Remove(friendRel1);
      // }
    }

    /// <summary>
    /// commit and save changes to the db
    /// only call this from the service, DO NOT CALL FROM REPO ITSELF
    /// Unit of work methology.
    /// </summary>
    public void SaveChanges()
    {
      this.dbContext.SaveChanges();
    }

    /// <summary>
    /// Get User with given friend code
    /// </summary>
    /// <param name="friendCode">Code of the friend</param>
    /// <returns>User with the given code</returns>
    public async Task<User> GetUserWithCode(string friendCode)
    {
      User user = await this.dbContext.User.FirstOrDefaultAsync(u => u.Code.Equals(friendCode));
      return user;
    }

    /// <summary>
    /// Add friend relation
    /// </summary>
    /// <param name="friend">senders unique id</param>
    public async Task<Friend> PlusFriend(Friend friend)
    {
      var result = await this.dbContext.Frineds.AddAsync(friend);
      return result.Entity;
    }

    /// <summary>
    /// Check whether friendship exist
    /// </summary>
    /// <param name="acceptedId">accepted user id</param>
    /// <param name="sharedId">shared user id</param>
    public async Task<Friend> checkFiendShip(int acceptedId, int sharedId)
    {
      var res = await this.dbContext.Frineds.FirstOrDefaultAsync(u => (u.AcceptedUser.Equals(acceptedId) && u.SharedUserId.Equals(sharedId)) || (u.AcceptedUser.Equals(sharedId) && u.SharedUserId.Equals(acceptedId)));
      return res;
    }
  }
}