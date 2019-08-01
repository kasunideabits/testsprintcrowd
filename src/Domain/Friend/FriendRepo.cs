namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using System;
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
    }
}