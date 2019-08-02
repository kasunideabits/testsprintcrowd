﻿namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Interface for sprint crowd frined service
    /// </summary>
    public interface IFriendService
    {
        /// <summary>
        /// Keep track friend request
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="friendId">user id for who receive the request</param>
        /// <param name="code">uniqe code for request</param>
        /// <returns>><see cref="AddFriendRequestResult">success or faild </see></returns>
        Task<string> AddFriendRequest(int userId, int friendId, int code);

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
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="frindId">user id of friend</param>
        Task RemoveFriend(int userId, int frindId);
    }
}