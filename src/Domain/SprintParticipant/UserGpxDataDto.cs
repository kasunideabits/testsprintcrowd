

namespace SprintCrowdBackEnd.Domain.SprintParticipant
{   /// <summary>
    /// DTO for hold GPX data for a user
    /// </summary>
    public class UserGpxDataDto
    {
        /// <summary>
        /// UserId for the GPX data 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// SprintId for the GPX data 
        /// </summary>
        public int SprintId { get; set; }
        /// <summary>
        /// GpxData for the GPX data 
        /// </summary>
        public string GpxData { get; set; }
    }
}
