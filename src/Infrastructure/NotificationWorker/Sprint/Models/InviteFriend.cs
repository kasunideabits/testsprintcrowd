﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    public class InviteFriend
    {
        public InviteFriend(int userId, string profilePicture, string userName, DateTime createdDate, int requestSenderId, string text)
        {
            this.UserId = userId;
            this.ProfilePicture = profilePicture;
            this.UserName = userName;
            this.CreatedDate = createdDate;
            this.RequestSenderId = requestSenderId;
            this.Text = text;
        }

        public int UserId { get; set; }
        public string ProfilePicture { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; }
        public int RequestSenderId { get; }
        public string Text { get; set; }



    }
}
