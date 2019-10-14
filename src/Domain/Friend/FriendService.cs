using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
  using System.Threading.Tasks;
  using System;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  ///  Implement <see cref="IFriendService" > interface </see>
  /// </summary>
  public class FriendService : IFriendService
  {
    /// <summary>
    /// Initialize <see cref="FriendService"> class </see>
    /// </summary>
    /// <param name="friendRepo">friend repository</param>
    public FriendService(IFriendRepo friendRepo)
    {
      this.FriendRepo = friendRepo;
    }

    private IFriendRepo FriendRepo { get; }

    /// <summary>
    /// Generate friend code
    /// </summary>
    /// <param name="userId">user id for who send the request</param>
    /// <param name="userCode">uniqe code for user</param>
    /// <returns>><see cref="GenerateFriendCodeResult">success or faild </see></returns>
    public async Task<string> GenerateFriendCode(int userId, string userCode)
    {
      try
      {
        var code = SCrowdUniqueKey.UniqFriendCode(userCode);
        await this.FriendRepo.GenerateFriendCode(userId, code);
        this.FriendRepo.SaveChanges();
        return GenerateFriendCodeResult.Success();
      }
      catch (Exception e)
      {
        throw new Application.ApplicationException(GenerateFriendCodeResult.Faild(), e);
      }
    }

    /// <summary>
    /// Add given user with matching friend code
    /// </summary>
    /// <param name="userId">resonder user id</param>
    /// <param name="friendCode">generate friend code</param>
    public async Task AddFriend(int userId, string friendCode)
    {
      var requesterCode = SCrowdUniqueKey.GetUserCode(friendCode);
      await this.FriendRepo.AddFriend(requesterCode, userId, friendCode);
      this.FriendRepo.SaveChanges();
      return;
    }

    /// <summary>
    /// Get firends for given user id
    /// </summary>
    /// <param name="userId">user id to lookup</param>
    /// <returns><see cref="FriendListDto">list of users</see></returns>
    // public async Task<FriendListDto> GetFriends(int userId)
    // {
    // var friends = await this.FriendRepo.GetFriends(userId);
    // var result = new FriendListDto();
    // friends.ForEach(u => result.AddFriend(u.Id, u.Name, u.ProfilePicture, u.Code));
    // return result;
    // }

    /// <summary>
    /// Remove friend from user list
    /// </summary>
    /// <param name="userId">user id for requester</param>
    /// <param name="friendId">user id of friend</param>
    public async Task RemoveFriend(int userId, int friendId)
    {
      await this.FriendRepo.RemoveFriend(userId, friendId);
      this.FriendRepo.SaveChanges();
      return;
    }

    /// <summary>
    /// Add given user with matching friend code
    /// </summary>
    /// <param name="userId">resonder user id</param>
    /// <param name="friendCode">generate friend code</param>
    public async Task<User> PlusFriend(int userId, string friendCode)
    {
      User user = await this.FriendRepo.GetUserWithCode(friendCode);

      if (user == null)
      {
        throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserCode, "Invalid code");
      }
      else
      {
        var isFriends = await this.FriendRepo.checkFiendShip((int)user.Id, (int)userId);
        if (isFriends == null)
        {
          Friend friend = new Friend();
          friend.AcceptedUserId = (int)user.Id;
          friend.SharedUserId = (int)userId;
          friend.CreatedTime = DateTime.Now;
          friend.UpdatedTime = DateTime.Now;
          await this.FriendRepo.PlusFriend(friend);
          this.FriendRepo.SaveChanges();
          return user;
        }
        else
        {
          throw new Application.SCApplicationException((int)FriendCustomErrorCodes.AlreadyFriends, "Already Friends");
        }
      }
    }
  }
}