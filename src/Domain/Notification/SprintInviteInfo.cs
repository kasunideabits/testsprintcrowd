namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System;
    using SprintCrowd.BackEnd.Application;

    public class SprintInviteInfo
    {
        public int SprintInviteId { get; set; }

        public int SprintId { get; set; }

        public string Name { get; set; }

        public int Distance { get; set; }

        public DateTime StartTime { get; set; }

        public SprintInvitationStatus Status { get; set; }
    }
}