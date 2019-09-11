namespace SprintCrowd.BackEnd.Common
{
    public class ParticipantBaseDto
    {
        public ParticipantBaseDto(int id, string name, string profilePicture, string city, string country, string countryCode)
        {
            this.Id = id;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Location = new LocationDto(city, country, countryCode);
        }

        public ParticipantBaseDto(int id, string name, string profilePicture, LocationDto location)
        {
            this.Id = id;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Location = location;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public LocationDto Location { get; set; }

    }
}