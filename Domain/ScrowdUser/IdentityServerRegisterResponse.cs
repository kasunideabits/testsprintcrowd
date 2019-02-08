using Newtonsoft.Json;

namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    /// <summary>
    /// Only contains basic user details
    /// slimmed down version of user
    /// this is the model returned by the identitiy server upon registeration
    /// </summary>
    public class IdentityServerRegisterResponse
    {
        [JsonProperty("statusCode", NullValueHandling = NullValueHandling.Include)]
        public int? StatusCode {get; set;}
        [JsonProperty("errorDescription")]
        public string ErrorDescription {get; set;}
        [JsonProperty("data", NullValueHandling = NullValueHandling.Include)]
        public BasicUser Data {get; set;}
    }

    public class BasicUser
    {
        [JsonProperty("email")]
        public string Email {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}
        [JsonProperty("userId")]
        public string UserId {get; set;}
    }
}