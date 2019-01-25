namespace SprintCrowd.Backend.Infrastructure.ExternalLogin
{
    using Newtonsoft.Json;

    public class FacebookProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}