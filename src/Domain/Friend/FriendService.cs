using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using System;

    /// <summary>
    ///  Implement <see cref="IFriendService" > interface </see>
    /// </summary>
    public class FriendService : IFriendService
    {
        public FriendService(IFriendRepo friendRepo)
        {
            this.FriendRepo = friendRepo;
        }

        private IFriendRepo FriendRepo { get; }

        /// <summary>
        /// Keep track friend request
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="friendId">user id for who receive the request</param>
        /// <param name="code">uniqe code for request</param>
        /// <returns>><see cref="AddFriendRequestResult">success or faild </see></returns>
        public async Task<string> AddFriendRequest(int userId, int friendId, int code)
        {
            try
            {
                await this.FriendRepo.AddFriendRequest(userId, friendId, code);
                return AddFriendRequestResult.Success();
            }
            catch (Exception e)
            {
                throw new Application.ApplicationException(AddFriendRequestResult.Faild(), e);
            }
        }
    }
}