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
        /// /// </summary>
        /// <param name="friendId">friend user id</param>
        /// <returns>Friend user details</returns>
        // TODO : Handle not found, Request status
        public async Task<User> GetFriend(int friendId)
        {
            Friend result = await this.Context.Frineds
                .Include(f => f.FriendOf)
                .FirstOrDefaultAsync(f => f.FriendId == friendId && f.RequestStatus == FriendRequestStatus.Accept);
            if (result != null)
            {
                return result.FriendOf;
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
        /// <returns><see cref="Friend">friend list</see></returns>
        // TODO : handle request status
        public async Task<List<Friend>> GetFriends(int userId)
        {
            return await this.Context.Frineds
                .Include(f => f.FriendOf)
                .Where(f => f.Id == userId && f.RequestStatus == FriendRequestStatus.Accept)
                .ToListAsync();
        }

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="frindId">user id of friend</param>
        public async Task RemoveFriend(int userId, int friendId)
        {
            var friendRel1 = this.Context.Frineds.FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);
            var friendRel2 = this.Context.Frineds.FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId);
            if (friendRel1 != null)
            {
                this.Context.Remove(friendRel1);
            }
            if (friendRel2 != null)
            {
                this.Context.Remove(friendRel1);
            }
        }
    }
}