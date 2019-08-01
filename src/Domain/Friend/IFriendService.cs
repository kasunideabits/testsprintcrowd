namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;

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
    }
}