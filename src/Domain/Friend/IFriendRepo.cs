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
        /// <param name="userId">user id</param>
        /// <param name="friendId">friend user id</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> reuqest status </see></param>
        /// <returns>Friend user details</returns>
        Task<FriendData> GetFriend(int userId, int friendId, FriendRequestStatus requestStatus);

        /// <summary>
        /// Get frind list for given user
        /// </summary>
        /// <param name="userId">user id for lookup friend</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> reuqest status </see></param>
        /// <returns><see cref="FriendData">friend list</see></returns>
        Task<List<FriendData>> GetFriends(int userId, FriendRequestStatus requestStatus);

        /// <summary>
        /// Get all friend request with filter request status
        /// </summary>
        /// <param name="userId">user id to lookup friends</param>
        /// <returns><see cref="FriendData"> friend list</see></returns>
        Task<List<FriendData>> GetAllFriends(int userId);

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="frindId">user id of friend</param>
        Task RemoveFriend(int userId, int frindId);

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        void SaveChanges();
    }
}