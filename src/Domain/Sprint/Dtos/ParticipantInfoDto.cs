namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    public class ParticipantInfoDto : ParticipantBaseDto
    {
        public ParticipantInfoDto(
            int id,
            string name,
            string profilePicture,
            string city,
            string country,
            string countryCode,
            string colorCode,
            bool creator,
            ParticipantStage stage,
            bool isInflencer = false) : base(id, name, profilePicture, city, country, countryCode)
        {
            this.ColorCode = colorCode;
            this.Creator = creator;
            this.Stage = stage;
            this.IsInfluencer = isInflencer;
        }

        public ParticipantInfoDto(
            User user,
            bool creator,
            ParticipantStage stage,
            bool isInflencer = false) : base(user.Id, user.Name, user.ProfilePicture, user.City, user.Country, user.CountryCode)
        {
            this.ColorCode = user.ColorCode;
            this.Creator = creator;
            this.Stage = stage;
            this.IsInfluencer = isInflencer;
        }

        public string ColorCode { get; }
        public bool Creator { get; }
        public ParticipantStage Stage { get; }
        public bool IsInfluencer { get; set; }

    }
}