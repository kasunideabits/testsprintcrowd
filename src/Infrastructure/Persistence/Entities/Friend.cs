namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;

    /// <summary>
    /// Class which indicates sprint crowd frineds
    /// </summary>

    public class Friend : BaseEntity
    {
        /// <summary>
        /// Gets or set unique id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set frined request sender
        /// </summary>
        public int SharedUserId { get; set; }

        /// <summary>
        ///  Gets or set friend request to
        /// </summary>
        public int AcceptedUserId { get; set; }

        /// <summary>
        /// Gets or set accept/decline time
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>user who acceptedrequest.</value>
        public virtual User AcceptedUser { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>user who shared request.</value>
        public virtual User SharedUser { get; set; }

    }
}