namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using System;
    using System.Collections.Generic;

    public class NotificationInfo //<T> where T : class, new()
    {
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public Notification Notification { get; set; }

        public int BadgeCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    
}