namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Roles.
    /// </summary>
    public class Roles : BaseEntity
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id.</value>
        public int Id { get; set; }

        /// <summary>
        ///  Gets or set iRole
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or set Description
        /// </summary>
        public string Description { get; set; }

    }
}