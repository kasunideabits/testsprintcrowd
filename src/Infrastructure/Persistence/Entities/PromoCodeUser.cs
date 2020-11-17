namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Promo Code User.
    /// </summary>
    public class PromoCodeUser : BaseEntity
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
        public int SprintId { get; set; }

        /// <summary>
        /// Promo Code
        /// </summary>
        public string PromoCode { get; set; }

        
    }
}