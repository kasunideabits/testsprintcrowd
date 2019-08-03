namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Interface for sprint crowd frined service
    /// </summary>
    public interface IFriendService
    {
        /// <summary>
        /// Generate friend code
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="userCode">uniqe code for user</param>
        /// <returns>><see cref="GenerateFriendCodeResult">success or faild </see></returns>
        Task<string> GenerateFriendCode(int userId, string userCode);

        /// <summary>
        /// Get friend details with given friend id
        /// </summary>
        /// <param name="userId">user id for lookup</param>
        /// <param name="friendId">friend user id for lookup</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> default is accept</see></param>
        /// <returns><see cref="FriendDto"> friend details </see></returns>
        Task<FriendDto> GetFriend(int userId, int friendId, FriendRequestStatus? requestStatus = FriendRequestStatus.Accept);

        /// <summary>
        /// Get frind list for given user
        /// </summary>
        /// <param name="userId">user id for lookup friend</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> default is accept</see></param>
        /// <returns><see cref="FriendListDto">friend list</see></returns>
        Task<FriendListDto> GetFriends(int userId, FriendRequestStatus? requestStatus);

        /// <summary>
        /// Get all friend request with filter request status
        /// </summary>
        /// <param name="userId">user id to lookup friends</param>
        /// <returns><see cref="FriendListDto"> friend list</see></returns>
        Task<FriendListDto> GetAllFriends(int userId);

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="frindId">user id of friend</param>
        Task RemoveFriend(int userId, int frindId);
    }
}