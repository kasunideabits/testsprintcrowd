namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;

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
            ParticipantStage stage) : base(id, name, profilePicture, city, country, countryCode)
        {
            this.ColorCode = colorCode;
            this.Creator = creator;
            this.Stage = stage;
        }
        public string ColorCode { get; }
        public bool Creator { get; }
        public ParticipantStage Stage { get; }
    }
}