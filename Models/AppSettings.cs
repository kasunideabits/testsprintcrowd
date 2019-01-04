namespace SprintCrowdBackEnd.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public PostGresSettings PostGres { get; set; }
    }

    public class PostGresSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}