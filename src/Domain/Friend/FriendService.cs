using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
  using System.Threading.Tasks;
  using System;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Common;
  using System.Collections.Generic;

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
    /// Add given user with matching friend code
    /// </summary>
    /// <param name="userId">resonder user id</param>
    /// <param name="friendCode">generate friend code</param>
    public async Task<AddFriendDTO> PlusFriend(int userId, string friendCode)
    {
      User user = await this.FriendRepo.GetUserWithCode(friendCode);

      if (user == null)
      {
        throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserCode, "Invalid code");
      }
      else
      {
        if ((int)user.Id == (int)userId)
        {
          throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserCode, "You cannot be friends with you");
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

            var addFriendDTO = new AddFriendDTO()
            {
              Name = user.Name,
              ProfilePicture = user.ProfilePicture
            };

            return addFriendDTO;
          }
          else
          {
            throw new Application.SCApplicationException((int)FriendCustomErrorCodes.AlreadyFriends, "Already Friends");
          }
        }

      }
    }

    /// <summary>
    /// Get all friends of loggedin user
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    public async Task<List<FriendListDTO>> AllFriends(int userId)
    {
      List<Friend> friends = await this.FriendRepo.GetAllFriends(userId);
      List<FriendListDTO> parts = new List<FriendListDTO>();
      friends.ForEach(obj =>
      {
        var friend = new FriendListDTO();

        if (obj.AcceptedUserId == userId)
        {
          friend.Id = obj.SharedUser.Id;
          friend.Name = obj.SharedUser.Name;
          friend.ProfilePicture = obj.SharedUser.ProfilePicture;
          friend.Code = obj.SharedUser.Code;
          friend.CreatedDate = obj.CreatedTime;
        }
        else if (obj.SharedUserId == userId)
        {
          friend.Id = obj.AcceptedUser.Id;
          friend.Name = obj.AcceptedUser.Name;
          friend.ProfilePicture = obj.AcceptedUser.ProfilePicture;
          friend.Code = obj.AcceptedUser.Code;
          friend.CreatedDate = obj.CreatedTime;
        }
        parts.Add(friend);
      });
      return parts;
    }

    /// <summary>
    /// Remove friend from friend list
    /// </summary>
    /// <param name="userId">loggedin user id</param>
    /// <param name="friendId">friend id</param>
    public async Task<RemoveFriendDTO> DeleteFriend(int userId, int friendId)
    {
      Friend isFriends = await this.FriendRepo.CheckAlreadyFriends(userId, friendId);

      if (isFriends == null)
      {
        throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserId, "Invalid user Id");
      }
      else
      {
        await this.FriendRepo.RemoveFriend(isFriends);

        this.FriendRepo.SaveChanges();

        var res = new RemoveFriendDTO()
        {
          Message = "User successfully removed"
        };

        return res;
      }
    }

    /// <summary>
    /// Get user with user id
    /// </summary>
    /// <param name="userId">user id of the user to be retrieved</param>
    public async Task<GetFriendDto> GetFriend(int userId)
    {

      var user = await this.FriendRepo.GetFriend(userId);

      if (user == null)
      {
        throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserId, "Invalid user Id");
      }
      else
      {
        var res = new GetFriendDto()
        {
          Id = user.Id,
          Name = user.Name,
          ProfilePicture = user.ProfilePicture,
          Code = user.Code,
          Email = user.Email
        };
        return res;
      }

    }
  }
}