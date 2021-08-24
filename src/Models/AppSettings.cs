namespace SprintCrowd.BackEnd.Models
{
  /// <summary>
  /// model for holding app settings in appSettings.json
  /// </summary>
  public class AppSettings
  {
    /// <summary>
    /// get or sets value.
    /// </summary>
    /// <value>jwt audience.</value>
    public string Audience { get; set; }
    /// <summary>
    /// get or sets value.
    /// </summary>
    /// <value>identity server url.</value>
    public string AuthorizationServer { get; set; }
    /// <summary>
    /// get or sets value.
    /// </summary>
    /// <value>endpoint for getting openid config from identity server.</value>
    public string OpenidConfigurationEndPoint { get; set; }
    /// <summary>
    /// get or sets value.
    /// </summary>
    /// <value>facebook app config.</value>
    public FacebookAppConfig FacebookApp { get; set; }
    /// <summary>
    /// get or sets value.
    /// </summary>
    /// <value>config for logging.</value>
    public LoggingConfig Logging { get; set; }
     /// <summary>
     /// Gps log api url
     /// </summary>
    public string GpsLogApi { get; set; }
    }

  /// <summary>
  /// config for facebook.
  /// </summary>
  public class FacebookAppConfig
  {
    /// <summary>
    /// get or sets value
    /// </summary>
    /// <value>facebook app id.</value>
    public string AppId { get; set; }
    /// <summary>
    /// get or sets value
    /// </summary>
    /// <value>facebook app secret.</value>
    public string AppSecret { get; set; }
  }

  /// <summary>
  /// logger config
  /// </summary>
  public class LoggingConfig
  {
    /// <summary>
    /// get or sets value
    /// </summary>
    /// <value>text file path where logs will be saved to.</value>
    public string LogPath { get; set; }
  }
}