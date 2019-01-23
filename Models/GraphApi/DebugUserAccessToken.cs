

namespace SprintCrowd.Backend.Models.GraphApi
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class DebugUserAccessToken
    {
        public TokenData Data { get; set; }
    }

    public class TokenData
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("application")]
        public string Application { get; set; }
        [JsonProperty("data_access_expires_at")]
        public int DataAcessExpiresAt { get; set; }
        [JsonProperty("expires_at")]
        public int ExpiresAt { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}