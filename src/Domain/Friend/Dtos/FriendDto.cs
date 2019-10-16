namespace SprintCrowd.BackEnd.Domain.Friend
{
    using System;
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
            DateTime createdDate) : base(id, name, profilePicture, city, country, countryCode)
        {
            this.Code = code;
            this.Email = email;
            this.ColoCode = colorCode;
            this.CreatedDate = createdDate;
        }
        public string Code { get; }
        public string ColoCode { get; }
        public string Email { get; }
        public DateTime CreatedDate { get; set; }
    }
}