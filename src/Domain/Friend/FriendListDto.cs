namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Collections.Generic;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Data transfer details for friend list
    /// </summary>
    public class FriendListDto
    {
        /// <summary>
        /// Initialize <see cref="FriendListDto"> class</see>
        /// </summary>
        public FriendListDto() => this.friends = new List<FriendDto>();

        /// <summary>
        /// Friend list with details
        /// </summary>
        public List<FriendDto> FriendList
        {
            get
            {
                return this.friends;
            }
        }

        private List<FriendDto> friends { get; set; }

        /// <summary>
        /// Add friend to a list
        /// </summary>
        /// <param name="requestId">request id</param>
        /// <param name="userId">user id for friend</param>
        /// <param name="name">name for friend</param>
        /// <param name="profilePicture">profile picutre for user</param>
        /// <param name="status">friend request status</param>
        public void AddFriend(
            int requestId,
            int userId,
            string name,
            string profilePicture,
            FriendRequestStatus status)
        {
            this.friends.Add(new FriendDto(requestId, userId, name, profilePicture, status));
        }

    }
}