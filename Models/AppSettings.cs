namespace SprintCrowdBackEnd.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public PostGresSettings PostGres { get; set; }
        public FacebookAppConfig FacebookApp { get; set; }
    }

    public class PostGresSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }

    public class FacebookAppConfig
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}