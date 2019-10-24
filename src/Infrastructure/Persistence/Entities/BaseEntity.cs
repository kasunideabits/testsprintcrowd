namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    /// <summary>
    ///  Base entity for data model
    /// </summary>
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}