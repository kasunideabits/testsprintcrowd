using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Friend
{
    public class FriendInviteDto
    {
        public int Id { get; set; }
        public int ToUserId { get; set; }
        public int FromUserId { get; internal set; }
    }
}
