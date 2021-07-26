namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// User Roles
    /// </summary>
    public class UserRoles : BaseEntity
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id.</value>
        public int Id { get; set; }

        /// <summary>
        ///  Gets or set id for participant
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or set sprint
        /// </summary>
        public int RoleId { get; set; }
        
    }
}