namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    public class JoinSprintResult
    {
        public int StatusCode { get; set; }

        public string Reason { get; set; }

        public JoinResult Result { get; set; }
    }

    public enum JoinResult
    {
        Success,

        Faild
    }
}