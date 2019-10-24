namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// Class represent exit sprint result
    /// </summary>
    public class ExitSprintResult
    {
        /// <summary>
        /// Indicate exit sprint success for not
        /// <see cref="ExitResult"> Exist Result </see>
        /// </summary>
        public ExitResult Result { get; set; }

        /// <summary>
        /// If fails, reason for failure
        /// </summary>
        public string Reason { get; set; }
    }

    /// <summary>
    /// Represent exit result enums
    /// </summary>
    public enum ExitResult
    {
        /// Success the exit
        Success,

        /// Faild to exit
        Faild
    }

    /// <summary>
    /// Exit sprint faild reason
    /// </summary>
    public sealed class ExitFaildReason
    {
        /// <summary>
        /// User id or sprint id dose not exist in the table
        /// </summary>
        public const string UserOrSprintNotMatch = "given sprint id or user id wrong";
    }
}