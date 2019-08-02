namespace SprintCrowd.BackEnd.Domain.Friend
{
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
        /// <summary>
        /// Initialize <see cref="FriendRepo"> class </see>
        /// </summary>
        /// <param name="context">database context</param>
        public FriendRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        /// <summary>
        /// Add friend request which store request
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="friendId">user id for who receive the request</param>
        /// <param name="code">uniqe code for request</param>
        public async Task AddFriendRequest(int userId, int friendId, int code)
        {
            Friend friend = new Friend()
            {
                UserId = userId,
                FriendId = friendId,
                Code = code,
                RequestStatus = FriendRequestStatus.Pendding,
                SendTime = DateTime.UtcNow,
            };
            await this.Context.Frineds.AddAsync(friend);
        }

        /// <summary>
        /// Get firend details with given friend id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="friendId">friend user id</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> reuqest status </see></param>
        /// <returns>Friend user details</returns>
        public async Task<FriendData> GetFriend(int userId, int friendId, FriendRequestStatus requestStatus)
        {
            Friend result = await this.Context.Frineds
                .Include(f => f.FriendOf)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f =>
                    ((f.UserId == userId && f.FriendId == friendId) ||
                        (f.FriendId == userId && f.UserId == friendId)) &&
                    f.RequestStatus == requestStatus);
            if (result != null && result.UserId == userId)
            {
                return new FriendData(result.Id, result.FriendOf, result.RequestStatus);
            }
            else if (result != null && result.User.Id == friendId)
            {
                return new FriendData(result.Id, result.User, result.RequestStatus);
            }
            else
            {
                throw new Application.ApplicationException("can find matching friend");
            }
        }

        /// <summary>
        /// Get frind list for given user
        /// </summary>
        /// <param name="userId">user id for lookup friend</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> reuqest status </see></param>
        /// <returns><see cref="FriendData">friend list</see></returns>
        public async Task<List<FriendData>> GetFriends(int userId, FriendRequestStatus requestStatus)
        {
            var user = await this.Context.User
                .Include(f => f.Friends)
                .ThenInclude(f => f.User)
                .Include(f => f.FriendRequester)
                .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == userId);
            List<FriendData> friends = new List<FriendData>();
            friends.AddRange(user.Friends.Where(f => f.RequestStatus == requestStatus).Select(f => new FriendData(f.Id, f.User, f.RequestStatus)));
            friends.AddRange(user.FriendRequester.Where(f => f.RequestStatus == requestStatus).Select(f => new FriendData(f.Id, f.User, f.RequestStatus)));
            return friends;
        }

        /// <summary>
        /// Get all friend request with filter request status
        /// </summary>
        /// <param name="userId">user id to lookup friends</param>
        /// <returns><see cref="FriendData"> friend list</see></returns>
        public async Task<List<FriendData>> GetAllFriends(int userId)
        {
            var user = await this.Context.User
                .Include(f => f.Friends)
                .ThenInclude(f => f.User)
                .Include(f => f.FriendRequester)
                .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == userId);
            List<FriendData> friends = new List<FriendData>();
            friends.AddRange(user.Friends.Select(f => new FriendData(f.Id, f.User, f.RequestStatus)));
            friends.AddRange(user.FriendRequester.Select(f => new FriendData(f.Id, f.User, f.RequestStatus)));
            return friends;
        }

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="friendId">user id of friend</param>
        public async Task RemoveFriend(int userId, int friendId)
        {
            var friendRel1 = await this.Context.Frineds
                .FirstOrDefaultAsync(f =>
                    (f.UserId == userId && f.FriendId == friendId) ||
                    (f.UserId == friendId && f.FriendId == userId));
            if (friendRel1 != null)
            {
                this.Context.Remove(friendRel1);
            }
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}