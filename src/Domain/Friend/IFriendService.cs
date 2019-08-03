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
        /// Add given user with matching friend code
        /// </summary>
        /// <param name="userId">resonder user id</param>
        /// <param name="friendCode">generate friend code</param>
        Task AddFriend(int userId, string friendCode);

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="frindId">user id of friend</param>
        Task RemoveFriend(int userId, int frindId);
    }
}