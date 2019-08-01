namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;

    /// <summary>
    /// Class which indicates sprint crowd frineds
    /// </summary>

    public class Friend
    {
        /// <summary>
        /// Gets or set unique id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set frined request sender
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  Gets or set friend request to
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Gets or set unique code for friend request
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or set request status for send
        /// request <see cref="FriendRequestStatus"> status </see>
        /// </summary>
        public FriendRequestStatus RequestStatus { get; set; }

        /// <summary>
        /// Gets or set request message recieve time
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Gets or set accept/decline time
        /// </summary>
        public DateTime StatusUpdatedTime { get; set; }

        /// <summary>
        /// Gets or set sender reference
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or set reciever reference
        /// </summary>
        public User FriendOf { get; set; }
    }
}