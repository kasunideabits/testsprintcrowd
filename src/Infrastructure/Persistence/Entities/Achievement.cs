using System.Collections.Generic;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Access tokens.
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>unique identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>user whom the achivement belongs to.</value>
        public User User { get; set; }
        /// <summary>
        /// Gets or set achivement Type
        /// </summary>
        /// <value>User reference</value>
        public int Type { get; set; }

        /// <summary>
        /// Gets or set notification reference
        /// </summary>
        public virtual List<Notifications> Notificatoins { get; set; }
    }
}