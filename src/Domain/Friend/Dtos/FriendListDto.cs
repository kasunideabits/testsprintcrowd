namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System;
    public class FriendListDto
    {
        public string Name { get; set; }
        public string ProfilePicture { get; set; }

        public int Id { get; set; }

        public string Code { get; set; }

        public DateTime CreatedDate { get; set; }
    }

}