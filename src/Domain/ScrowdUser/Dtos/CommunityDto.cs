using SprintCrowd.BackEnd.Domain.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.ScrowdUser.Dtos
{
    public class CommunityDto
    {
        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get user FriendDto
        /// </summary>
        public List<FriendDto> FriendDto { get; set; }

        /// <summary>
        /// Profile picture
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Useris friend of mine
        /// </summary>
        public bool IsFriendOfMine { get; set; }
    }
}
