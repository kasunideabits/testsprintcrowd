namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System.Collections.Generic;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Achievement table attributes
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>unique identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set user id who achieved the achievement
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or set achivement Type
        /// </summary>
        /// <value>User reference</value>
        public AchievementType Type { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>user whom the achivement belongs to.</value>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or set notification reference
        /// </summary>
        public virtual List<Notification> Notificatoins { get; set; }
    }
}