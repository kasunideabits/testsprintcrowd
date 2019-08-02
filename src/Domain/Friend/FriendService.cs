using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Implement <see cref="IFriendService" > interface </see>
    /// </summary>
    public class FriendService : IFriendService
    {
        /// <summary>
        /// Initialize <see cref="FriendService" class> </see>
        /// </summary>
        /// <param name="friendRepo">friend repository</param>
        public FriendService(IFriendRepo friendRepo)
        {
            this.FriendRepo = friendRepo;
        }

        private IFriendRepo FriendRepo { get; }

        /// <summary>
        /// Keep track friend request
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="friendId">user id for who receive the request</param>
        /// <param name="code">uniqe code for request</param>
        /// <returns>><see cref="AddFriendRequestResult">success or faild </see></returns>
        public async Task<string> AddFriendRequest(int userId, int friendId, int code)
        {
            try
            {
                await this.FriendRepo.AddFriendRequest(userId, friendId, code);
                this.FriendRepo.SaveChanges();
                return AddFriendRequestResult.Success();
            }
            catch (Exception e)
            {
                throw new Application.ApplicationException(AddFriendRequestResult.Faild(), e);
            }
        }

        /// <summary>
        /// Get friend details with given friend id
        /// </summary>
        /// <param name="friendId">friend user id for lookup</param>
        /// <returns><see cref="FriendDto"> friend details </see></returns>
        public async Task<FriendDto> GetFriend(int friendId)
        {
            User friend = await this.FriendRepo.GetFriend(friendId);
            return new FriendDto(friend.Id, friend.Name, friend.ProfilePicture);
        }

        /// <summary>
        /// Get frind list for given user
        /// </summary>
        /// <param name="userId">user id for lookup friend</param>
        /// <returns><see cref="FriendListDto">friend list</see></returns>
        public async Task<FriendListDto> GetFriends(int userId)
        {
            var friends = await this.FriendRepo.GetFriends(userId);
            var friendsList = new FriendListDto();
            friends.ForEach(f => friendsList.AddFriend(f.User.Id, f.User.Name, f.User.ProfilePicture));
            return friendsList;
        }

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="friendId">user id of friend</param>
        public async Task RemoveFriend(int userId, int friendId)
        {
            await this.FriendRepo.RemoveFriend(userId, friendId);
            return;
        }
    }
}