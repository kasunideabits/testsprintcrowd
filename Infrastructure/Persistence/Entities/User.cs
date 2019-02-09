namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// User model.
    /// </summary>
    public class User
    {
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>unique id for the user.</value>
        public int Id { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>facebook user id.</value>
        public string FacebookUserId { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>email of the user.</value>
        public string Email { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>name of the user.</value>
        public string Name { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>profile picture url of the user.</value>
        public string ProfilePicture { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>list of events the user has participated and participating on.</value>
        public List<Event> Events { get; set; }
    }
}