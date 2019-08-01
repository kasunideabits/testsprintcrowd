namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Interface for handle friend
    /// </summary>
    public interface IFriendRepo
    {
        /// <summary>
        /// Add friend request which store request
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="friendId">user id for who receive the request</param>
        /// <param name="code">uniqe code for request</param>
        Task AddFriendRequest(int userId, int friendId, int code);

        /// <summary>
        /// Get firend details with given friend id
        /// </summary>
        /// <param name="friendId">friend user id</param>
        /// <returns>Friend user details</returns>
        Task<User> GetFriend(int friendId);

        /// <summary>
        /// Get frind list for given user
        /// </summary>
        /// <param name="userId">user id for lookup friend</param>
        /// <returns><see cref="FriendListDto">friend list</see></returns>

        Task<List<Friend>> GetFriends(int userId);
    }
}