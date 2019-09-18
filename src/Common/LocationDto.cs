namespace SprintCrowd.BackEnd.Common
{
    public class LocationDto
    {
        public LocationDto(string city, string country, string countryCode)
        {
            this.City = city;
            this.Country = country;
            this.CountryCode = countryCode;
        }

        public string City { get; }
        public string Country { get; }
        public string CountryCode { get; }

    }
}