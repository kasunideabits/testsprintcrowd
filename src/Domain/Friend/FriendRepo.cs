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
        /// Generate friend request code and store
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="code">uniqe code for request</param>
        public async Task GenerateFriendCode(int userId, string code)
        {
            Friend friend = new Friend()
            {
                UserId = userId,
                Code = code,
                GenerateTime = DateTime.UtcNow,
            };
            await this.Context.Frineds.AddAsync(friend);
        }

        /// <summary>
        /// Add given user with matching friend code
        /// </summary>
        /// <param name="userCode">senders unique id</param>
        /// <param name="friendId">reponders user id</param>
        /// <param name="friendCode">generate friend code</param>
        public async Task AddFriend(string userCode, int friendId, string friendCode)
        {
            var requester = await this.Context.Frineds
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Code == friendCode && f.User.Code == userCode);
            if (requester == null)
            {
                throw new Application.ApplicationException(
                    (int)FriendRequestActionResult.RequestCodeNotFound,
                    "Invalid request code");
            }
            else
            {
                if (requester.FriendId == null)
                {
                    requester.FriendId = friendId;
                    requester.StatusUpdatedTime = DateTime.UtcNow;
                    return;
                }
                else if (requester.FriendId != null)
                {
                    throw new Application.ApplicationException(
                        (int)FriendRequestActionResult.AlreadyUsedCode,
                        "Already used code");
                }
            }
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