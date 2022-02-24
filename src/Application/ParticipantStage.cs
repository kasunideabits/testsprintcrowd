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
        COMPLETED = 3,

        PENDING = 4,

        DECLINE = 5,
    }

    /// <summary>
    /// Program Participant Stage
    /// </summary>
    public enum ProgramParticipantStage
    {
        /// <summary>
        /// pending the program
        /// </summary>
        PENDING = 0,

        /// <summary>
        /// joined the program
        /// </summary>
        JOINED = 1,


        /// <summary>
        /// quit the program
        /// </summary>
        QUIT = 2,

        /// <summary>
        /// completed the sprint
        /// </summary>
        COMPLETED = 3,

       
    }
}