using Newtonsoft.Json;

namespace SprintCrowdBackEnd.Models.GraphApi
{
    public class OAuthAppAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}