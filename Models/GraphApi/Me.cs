using Newtonsoft.Json;

namespace SprintCrowdBackEnd.Models.GraphApi
{
    public class Me
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        public FbProfilePicture ProfilePicture { get; set; }

    }

    public class FbProfilePicture
    {
        [JsonProperty("data")]
        public Data PictureData { get; set ; }
        public class Data
        {
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }
}