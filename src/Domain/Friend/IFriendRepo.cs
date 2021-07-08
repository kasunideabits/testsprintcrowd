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
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Get User with given friend code
        /// </summary>
        /// <param name="friendCode">generate friend code</param>
        /// <returns><see cref="User">user details</see></returns>
        Task<User> GetUserWithCode(string friendCode);

        /// <summary>
        /// Add friend relation
        /// </summary>
        /// <param name="friend">senders unique id</param>
        Task<Friend> PlusFriend(Friend friend);

        /// <summary>
        /// Check whether friendship exist
        /// </summary>
        /// <param name="acceptedId">accepted user id</param>
        /// <param name="sharedId">shared user id</param>
        Task<Friend> checkFiendShip(int acceptedId, int sharedId);

        /// <summary>
        /// Add friend relation
        /// </summary>
        /// <param name="userId">loggedin user id</param>
        Task<List<Friend>> GetAllFriends(int userId);

        /// <summary>
        /// Check whether given user is a friend of loggedin user
        /// </summary>
        /// <param name="userId">loggedin user id</param>
        /// <param name="friendId">friend id</param>
        Task<Friend> CheckAlreadyFriends(int userId, int friendId);

        /// <summary>
        /// Remove friend from friend list
        /// </summary>
        /// <param name="friend">friend to be removed</param>
        Task<Friend> RemoveFriend(Friend friend);

        /// <summary>
        /// Get user with user id
        /// </summary>
        /// <param name="userId">user id of the user to be retrieved</param>
        Task<User> GetFriend(int userId);

        /// <summary>
        /// Get friendship with given user Id
        /// </summary>
        /// <param name="userId">friend user id</param>
        /// <returns></returns>
        Task<Friend> GetFriendship(int userId);

        /// <summary>
        /// Send friend invite from internal app
        /// </summary>
        /// <param name="invite">FriendInvite db entity object</param>
        /// <returns></returns>
        Task<FriendInvite> InviteFriend(FriendInvite invite);

        /// <summary>
        /// List invites for logged user
        /// </summary>
        /// <param name="userId">logged user id</param>
        /// <returns></returns>
        Task<List<FriendInvite>> InvitationsListRecievedByUser(int userId);

        /// <summary>
        /// Invitations list sent by user
        /// </summary>
        /// <param name="userId">logged user id</param>
        /// <returns></returns>
        /// 
        Task<List<FriendInvite>> InvitationsListSentByUser(int userId);
    }
}