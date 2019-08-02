﻿using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Threading.Tasks;
    using System;
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
                this.FriendRepo.SaveChanges();
                return AddFriendRequestResult.Success();
            }
            catch (Exception e)
            {
                throw new Application.ApplicationException(AddFriendRequestResult.Faild(), e);
            }
        }

        /// <summary>
        /// Get friend details with given friend id
        /// </summary>
        /// <param name="userId">user id for lookup</param>
        /// <param name="friendId">friend user id for lookup</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> default is accept</see></param>
        /// <returns><see cref="FriendDto"> friend details </see></returns>

        public async Task<FriendDto> GetFriend(int userId, int friendId, FriendRequestStatus? requestStatus)
        {
            var status = requestStatus?? FriendRequestStatus.Accept;
            var friend = await this.FriendRepo.GetFriend(userId, friendId, status);
            return new FriendDto(friend.RequestId, friend.User.Id, friend.User.Name, friend.User.ProfilePicture, status);
        }

        /// <summary>
        /// Get frind list for given user
        /// </summary>
        /// <param name="userId">user id for lookup friend</param>
        /// <param name="requestStatus"><see cref="FriendRequestStatus"> default is accept</see></param>
        /// <returns><see cref="FriendListDto">friend list</see></returns>
        public async Task<FriendListDto> GetFriends(int userId, FriendRequestStatus? requestStatus)
        {
            var status = requestStatus?? FriendRequestStatus.Accept;
            var friends = await this.FriendRepo.GetFriends(userId, status);
            var friendsList = new FriendListDto();
            friends.ForEach(f => friendsList.AddFriend(f.RequestId, f.User.Id, f.User.Name, f.User.ProfilePicture, status));
            return friendsList;
        }

        /// <summary>
        /// Get all friend request with filter request status
        /// </summary>
        /// <param name="userId">user id to lookup friends</param>
        /// <returns><see cref="FriendListDto"> friend list</see></returns>
        public async Task<FriendListDto> GetAllFriends(int userId)
        {
            var friends = await this.FriendRepo.GetAllFriends(userId);
            var friendsList = new FriendListDto();
            friends.ForEach(f => friendsList.AddFriend(f.RequestId, f.User.Id, f.User.Name, f.User.ProfilePicture, f.Status));
            return friendsList;
        }

        /// <summary>
        /// Accept friend request
        /// </summary>
        /// <param name="requestId">unique id for friend request</param>
        /// <param name="userId">user id who send the request</param>
        /// <param name="friendId">user id who receive the request</param>
        /// <param name="code">unique code for friend request</param>
        // TODO  notification:
        public async Task Accept(int requestId, int userId, int friendId, int code)
        {
            await this.FriendRepo.Accept(requestId, userId, friendId, code);
            this.FriendRepo.SaveChanges();
            return;
        }

        /// <summary>
        /// Decline friend request
        /// </summary>
        /// <param name="requestId">unique id for friend request</param>
        /// <param name="userId">user id who send the request</param>
        /// <param name="friendId">user id who receive the request</param>
        /// <param name="code">unique code for friend request</param>
        // TODO  notification:
        public async Task Decline(int requestId, int userId, int friendId, int code)
        {
            await this.FriendRepo.Decline(requestId, userId, friendId, code);
            this.FriendRepo.SaveChanges();
            return;
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