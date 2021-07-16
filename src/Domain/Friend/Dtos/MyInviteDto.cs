using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    public class InviteDto
    {
        public List<InviteRecieved> InviteRecievedList { get; set; }

        public List<InviteSend> InviteSendList { get; set; }

    }

    public class InviteRecieved
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string ProfilePicture { get; internal set; }
    }

    public class InviteSend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string ProfilePicture { get; internal set; }
    }
}
