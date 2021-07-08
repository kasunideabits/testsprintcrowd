
namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    public class FriendInvite : BaseEntity
    {
        /// <summary>
        /// Gets or set unique id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set friend request sender
        /// </summary>
        public int FromUserId { get; set; }

        /// <summary>
        /// Gets or set friend request reciever
        /// </summary>
        public int ToUserId { get; set; }

        /// <summary>
        /// Invitation is accepted or not.
        /// </summary>
        public bool Accepted { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>user who send friend request.</value>
        public virtual User FromUser { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>user who recieved friend request.</value>
        public virtual User ToUser { get; set; }


    }
}
