namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
  using Newtonsoft.Json;

  /// <summary>
  /// Only contains basic user details.
  /// slimmed down version of user.
  /// this is the model returned by the identitiy server upon registeration.
  /// </summary>
  public class IdentityServerRegisterResponse
  {
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>status code for sent by the identity server.</value>
    [JsonProperty("statusCode")]
    public int? StatusCode { get; set; }
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>description of error if error occured.</value>
    [JsonProperty("errorDescription")]
    public string ErrorDescription { get; set; }
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>data returned by the identity server.</value>
    [JsonProperty("data", NullValueHandling = NullValueHandling.Include)]
    public BasicUser Data { get; set; }
  }

  /// <summary>
  /// Slimmed down version of user returned by identity server.
  /// </summary>
  public class BasicUser
  {
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>email of the user.</value>
    [JsonProperty("email")]
    public string Email { get; set; }
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>name of the user.</value>
    [JsonProperty("name")]
    public string Name { get; set; }
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>user if of the user.</value>
    [JsonProperty("userId")]
    public string UserId { get; set; }
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>link to the profile picture.</value>
    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; set; }
  }
}