namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System.Collections.Generic;

    /// <summary>
    /// Data transfer details for friend list
    /// </summary>
    public class FriendListDto
    {
        /// <summary>
        /// Initialize <see cref="FriendListDto"> class</see>
        /// </summary>
        /// <typeparam name="FriendDto"> friend details</typeparam>
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
        /// <param name="userId">user id for friend</param>
        /// <param name="name">name for friend</param>
        /// <param name="profilePicture">profile picutre for user</param>
        public void AddFriend(int userId, string name, string profilePicture)
        {
            this.friends.Add(new FriendDto(userId, name, profilePicture));
        }

    }
}