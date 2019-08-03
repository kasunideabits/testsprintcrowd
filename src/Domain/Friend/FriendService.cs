using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Implement <see cref="IFriendService" > interface </see>
    /// </summary>
    public class FriendService : IFriendService
    {
        /// <summary>
        /// Initialize <see cref="FriendService"> class </see>
        /// </summary>
        /// <param name="friendRepo">friend repository</param>
        public FriendService(IFriendRepo friendRepo)
        {
            this.FriendRepo = friendRepo;
        }

        private IFriendRepo FriendRepo { get; }

        /// <summary>
        /// Generate friend code
        /// </summary>
        /// <param name="userId">user id for who send the request</param>
        /// <param name="userCode">uniqe code for user</param>
        /// <returns>><see cref="GenerateFriendCodeResult">success or faild </see></returns>
        public async Task<string> GenerateFriendCode(int userId, string userCode)
        {
            try
            {
                var code = SCrowdUniqueKey.UniqFriendCode(userCode);
                await this.FriendRepo.GenerateFriendCode(userId, code);
                this.FriendRepo.SaveChanges();
                return GenerateFriendCodeResult.Success();
            }
            catch (Exception e)
            {
                throw new Application.ApplicationException(GenerateFriendCodeResult.Faild(), e);
            }
        }

        /// <summary>
        /// Remove friend from user list
        /// </summary>
        /// <param name="userId">user id for requester</param>
        /// <param name="friendId">user id of friend</param>
        public async Task RemoveFriend(int userId, int friendId)
        {
            await this.FriendRepo.RemoveFriend(userId, friendId);
            this.FriendRepo.SaveChanges();
            return;
        }
    }
}