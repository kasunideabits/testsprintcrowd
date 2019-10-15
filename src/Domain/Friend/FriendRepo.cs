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

    /// <summary>
    /// Get all friend of logged in user
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    /// <returns>All friends</returns>
    public async Task<List<Friend>> GetAllFriends(int userId)
    {
      return await this.dbContext.Frineds
          .Include(s => s.AcceptedUser)
          .Where(s => s.AcceptedUserId == userId || s.SharedUserId == userId)
          .ToListAsync();
    }

    /// <summary>
    /// Remove friend from friend list
    /// </summary>
    /// <param name="friend">friend to be removed</param>
    public async Task<Friend> RemoveFriend(Friend friend)
    {
      Friend friendship = await this.dbContext.Frineds.FindAsync(friend.Id);
      this.dbContext.Remove(friendship);
      return null;
    }

    /// <summary>
    /// Check whether given user is a friend of loggedin user
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    /// <param name="friendId">friend id</param>
    public async Task<Friend> CheckAlreadyFriends(int userId, int friendId)
    {
      return await this.dbContext.Frineds
      .FirstOrDefaultAsync(s => (s.AcceptedUserId == userId && s.SharedUserId == friendId) || (s.AcceptedUserId == friendId && s.SharedUserId == userId));
    }

    /// <summary>
    /// Get user with user id
    /// </summary>
    /// <param name="userId">user id of the user to be retrieved</param>
    public async Task<User> GetFriend(int userId)
    {
      User user = await this.dbContext.User.FindAsync(userId);
      return user;
    }
  }
}