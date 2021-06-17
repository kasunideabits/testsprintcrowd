namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Promo Code User.
    /// </summary>
    public class UserAppVersionInfo : BaseEntity
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id.</value>
        public int Id { get; set; }

        /// <summary>
        ///  Gets or set AppOS for user app
        /// </summary>
        public string AppOS { get; set; }

        
        /// <summary>
        /// Gets or set App Version for user app
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        ///  Gets or set Is Force Upgrade for user app
        /// </summary>
        public bool IsForceUpgrade { get; set; }

        /// <summary>
        ///  Gets or set Upgrade Priority for user app
        /// </summary>
        public int UpgradePriority { get; set; }
    }
}