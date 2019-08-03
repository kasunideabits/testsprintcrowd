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