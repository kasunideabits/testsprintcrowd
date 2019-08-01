namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;

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
    }
}