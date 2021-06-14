namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;

    public class FriendDto : ParticipantBaseDto
    {
        public FriendDto(
            int id,
            string name,
            string profilePicture,
            string code,
            string email,
            string city,
            string country,
            string countryCode,
            string colorCode,
            DateTime createdDate,
            UserShareType userShareType = UserShareType.None) : base(id, name, profilePicture, city, country, countryCode)
        {
            this.Code = code;
            this.Email = email;
            this.ColorCode = colorCode;
            this.CreatedDate = createdDate;
            this.UserShareType = userShareType;
        }
        public string Code { get; }
        public string ColorCode { get; }
        public string Email { get; }
        public DateTime CreatedDate { get; set; }
        public UserShareType UserShareType { get; set; }
    }
}