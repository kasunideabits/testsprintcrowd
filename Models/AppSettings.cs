namespace SprintCrowd.Backend.Models
{
    public class AppSettings
    {
        public string Audience { get; set; }
        public string AuthorizationServer { get; set; }
        public string OpenidConfigurationEndPoint { get; set; }
        public FacebookAppConfig FacebookApp { get; set; }
        public LoggingConfig Logging { get; set; }
    }

    public class FacebookAppConfig
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }

    public class LoggingConfig
    {
        public string LogPath { get; set; }
    }
}