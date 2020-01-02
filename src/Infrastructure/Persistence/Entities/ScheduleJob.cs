namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Schedule job entity
    /// </summary>
    public class ScheduleJob : BaseEntity
    {
        /// <summary>
        /// Primary index
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Target id , for sprint shedule job it will be sprint id
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Hangire job id
        /// </summary>
        public string JobId { get; set; }
    }
}