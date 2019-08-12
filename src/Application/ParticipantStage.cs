namespace SprintCrowd.BackEnd.Application
{
    /// <summary>
    /// Enums for participant stages
    /// </summary>
    public enum ParticipantStage
    {
        /// <summary>
        /// joined the race
        /// </summary>
        JOINED = 0,

        /// <summary>
        /// marked attendence
        /// </summary>
        MARKED_ATTENDENCE = 1,

        /// <summary>
        /// quit the race
        /// </summary>
        QUIT = 2,

        /// <summary>
        /// completed the sprint
        /// </summary>
        COMPLETED = 3
    }
}