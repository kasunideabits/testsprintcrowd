using System.ComponentModel.DataAnnotations;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Access tokens.
    /// </summary>
    public class AccessToken : BaseEntity
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>actual token.</value>
        [MaxLength(2000)]
        public string Token { get; set; }
    }
}