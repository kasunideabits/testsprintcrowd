namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Interface for sprint crowd frined service
    /// </summary>
    public interface IFriendService
    {

        /// <summary>
        /// Add given user with matching friend code
        /// </summary>
        /// <param name="userId">resonder user id</param>
        /// <param name="friendCode">generate friend code</param>
        /// <returns><see cref="AddFriendDto"></see> and reason</returns>
        Task<FriendDto> PlusFriend(User userAccept, string friendCode);

        /// <summary>
        /// Get all friends of loggedin user
        /// </summary>
        /// <param name="userId">loggedin user id</param>
        /// <returns><see cref="FriendDto"></see> and reason</returns>
        Task<List<FriendDto>> AllFriends(int userId);

        /// <summary>
        /// Remove friend from friend list
        /// </summary>
        /// <param name="userId">loggedin user id</param>
        /// <param name="friendId">friend id</param>
        Task<RemoveFriendDto> DeleteFriend(int userId, int friendId);

        /// <summary>
        /// Get user with user id
        /// </summary>
        /// <param name="userId">user id of the user to be retrieved</param>
        Task<FriendDto> GetFriend(int userId);
    }
}