namespace SprintCrowd.Backend.Models.GraphApi
{
    using Newtonsoft.Json;

    public class OAuthAppAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}