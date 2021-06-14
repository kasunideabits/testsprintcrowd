using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;

    /// <summary>
    ///  Implement <see cref="IFriendService" > interface </see>
    /// </summary>
    public class FriendService : IFriendService
    {
        /// <summary>
        /// Initialize <see cref="FriendService"> class </see>
        /// </summary>
        /// <param name="friendRepo">friend repository</param>
        public FriendService(IFriendRepo friendRepo, INotificationClient notificationClient)
        {
            this.FriendRepo = friendRepo;
            this.NotificationClient = notificationClient;
        }

        private IFriendRepo FriendRepo { get; }
        private INotificationClient NotificationClient { get; }
        /// <summary>
        /// Add given user with matching friend code
        /// </summary>
        /// <param name="userId">resonder user id</param>
        /// <param name="friendCode">generate friend code</param>
        public async Task<FriendDto> PlusFriend(User userAccept, string friendCode)
        {
            User user = await this.FriendRepo.GetUserWithCode(friendCode);

            if (user == null)
            {
                throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserCode, "Invalid code");
            }
            else
            {
                if ((int)user.Id == (int)userAccept.Id)
                {
                    throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserCode, "You cannot be friends with you");
                }
                else
                {
                    var isFriends = await this.FriendRepo.checkFiendShip((int)user.Id, (int)userAccept.Id);
                    if (isFriends == null)
                    {
                        Friend friend = new Friend();
                        friend.AcceptedUserId = (int)user.Id;
                        friend.SharedUserId = (int)userAccept.Id;
                        friend.CreatedDate = DateTime.Now;
                        friend.UpdatedTime = DateTime.Now;
                        await this.FriendRepo.PlusFriend(friend);
                        this.FriendRepo.SaveChanges();

                        this.NotificationClient.SprintNotificationJobs.AcceptRequest(
                            userAccept.Id,
                            userAccept.Name,
                            userAccept.ProfilePicture,
                            userAccept.Code,
                            userAccept.Email,
                            userAccept.City,
                            userAccept.Country,
                            userAccept.CountryCode,
                            userAccept.ColorCode,
                            userAccept.CreatedDate,
                            user.Id);

                        return new FriendDto(
                            user.Id,
                            user.Name,
                            user.ProfilePicture,
                            user.Code,
                            user.Email,
                            user.City,
                            user.Country,
                            user.CountryCode,
                            user.ColorCode,
                            friend.CreatedDate);
                    }
                    else
                    {
                        throw new Application.SCApplicationException((int)FriendCustomErrorCodes.AlreadyFriends, "Already Friends");
                    }
                }

            }
        }

        /// <summary>
        /// Get all friends of loggedin user
        /// </summary>
        /// <param name="userId">loggedin user id</param>
        public async Task<List<FriendDto>> AllFriends(int userId)
        {
            List<Friend> friends = await this.FriendRepo.GetAllFriends(userId);
            List<FriendDto> parts = new List<FriendDto>();
            friends.ForEach(obj =>
            {
                if (obj.AcceptedUserId == userId && parts.Find(x => x.Id == obj.SharedUser.Id) == null) 
            {
                    parts.Add(new FriendDto(
                        obj.SharedUser.Id,
                        obj.SharedUser.Name,
                        obj.SharedUser.ProfilePicture,
                        obj.SharedUser.Code,
                        obj.SharedUser.Email,
                        obj.SharedUser.City,
                        obj.SharedUser.Country,
                        obj.SharedUser.CountryCode,
                        obj.SharedUser.ColorCode,
                        obj.CreatedDate,
                        obj.SharedUser.UserShareType));
                }

                else if (obj.SharedUserId == userId && parts.Find(x => x.Id == obj.AcceptedUser.Id) == null)
                {
                    parts.Add(new FriendDto(
                        obj.AcceptedUser.Id,
                        obj.AcceptedUser.Name,
                        obj.AcceptedUser.ProfilePicture,
                        obj.AcceptedUser.Code,
                        obj.AcceptedUser.Email,
                        obj.AcceptedUser.City,
                        obj.AcceptedUser.Country,
                        obj.AcceptedUser.CountryCode,
                        obj.AcceptedUser.ColorCode,
                        obj.CreatedDate,
                        obj.AcceptedUser.UserShareType));
                }
            });
            return parts;
        }

        /// <summary>
        /// Remove friend from friend list
        /// </summary>
        /// <param name="userId">loggedin user id</param>
        /// <param name="friendId">friend id</param>
        public async Task<RemoveFriendDto> DeleteFriend(int userId, int friendId)
        {
            Friend isFriends = await this.FriendRepo.CheckAlreadyFriends(userId, friendId);

            if (isFriends == null)
            {
                throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserId, "Invalid user Id");
            }
            else
            {
                await this.FriendRepo.RemoveFriend(isFriends);

                this.FriendRepo.SaveChanges();

                return new RemoveFriendDto("User successfully removed");
            }
        }

        /// <summary>
        /// Get user with user id
        /// </summary>
        /// <param name="userId">user id of the user to be retrieved</param>
        public async Task<FriendDto> GetFriend(int userId)
        {

            var friend = await this.FriendRepo.GetFriendship(userId);
            if (friend == null)
            {
                throw new Application.SCApplicationException((int)FriendCustomErrorCodes.InvalidUserId, "Invalid user Id");
            }
            else
            {
                if (friend.SharedUserId == userId)
                {
                    return new FriendDto(
                        friend.SharedUser.Id,
                        friend.SharedUser.Name,
                        friend.SharedUser.ProfilePicture,
                        friend.SharedUser.Code,
                        friend.SharedUser.Email,
                        friend.SharedUser.City,
                        friend.SharedUser.Country,
                        friend.SharedUser.CountryCode,
                        friend.SharedUser.ColorCode,
                        friend.CreatedDate);
                }
                else
                {
                    return new FriendDto(
                        friend.AcceptedUser.Id,
                        friend.AcceptedUser.Name,
                        friend.AcceptedUser.ProfilePicture,
                        friend.AcceptedUser.Code,
                        friend.AcceptedUser.Email,
                        friend.AcceptedUser.City,
                        friend.AcceptedUser.Country,
                        friend.AcceptedUser.CountryCode,
                        friend.AcceptedUser.ColorCode,
                        friend.CreatedDate);
                }
            }
        }
    }
}