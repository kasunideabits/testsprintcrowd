namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    using SprintCrowd.BackEnd.Common;

    public class FriendInSprintDto : ParticipantBaseDto
    {
        public FriendInSprintDto(int id, string name, string profilePicture, string city, string country, string countryCode, string colorCode, bool isInSprint) : base(id, name, profilePicture, city, country, countryCode)
        {
            this.IsInSprint = isInSprint;
        }

        public string ColorCode { get; }
        public bool IsInSprint { get; }
    }
}