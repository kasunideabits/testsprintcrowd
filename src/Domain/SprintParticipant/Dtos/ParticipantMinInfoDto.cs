using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// class for indicate sprint participant minimal info
    /// </summary>
    public class ParticipantMinInfoDto
    {
        /// <summary>
        /// Participant id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Participant name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Participant name
        /// </summary>
        public string Stage { get; set; }
    }
}