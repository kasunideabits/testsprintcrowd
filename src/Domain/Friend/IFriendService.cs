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

        /// <summary>
        /// Friend invite send through the app
        /// </summary>
        /// <param name="FromUserId"></param>
        /// <param name="ToUserId"></param>
        /// <param name="isCommunity"></param>
        /// <returns></returns>
        Task<FriendInviteDto> InviteFriend(int FromUserId, int ToUserId, bool isCommunity);

        /// <summary>
        /// List invites for logged user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<InviteDto> InviteList(int userId, bool isCommunity);


        /// <summary>
        /// Friend invite accept
        /// </summary>
        /// <param name="id">id of the invite</param>
        /// <param name="isCommunity">isCommunity or general notification</param>
        /// <returns>Successfull updated record</returns>
        Task<FriendInviteDto> InviteAccept(int id, bool isCommunity);



        /// <summary>
        /// Remove an invitation
        /// </summary>
        /// <param name="id">friend to be removed</param>
        /// <param name="isCommunity">General notification or community</param>
        Task<bool> RemoveInvitation(int id, bool isCommunity);

        /// <summary>
        /// Friend invitation list sent by user
        /// </summary>
        /// <param name="userId">logged user id</param>
        /// <returns></returns>
        Task<List<FriendInvite>> InvitationsListSentByUser(int userId);
       }
}