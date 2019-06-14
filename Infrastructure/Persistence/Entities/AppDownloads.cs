namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// model for holding download device info
    /// </summary>
    /// <value></value>
    public class AppDownloads
    {
        /// <summary>
        /// AppDownload id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// uuid of the device
        /// </summary>
        /// <value></value>
        public int DeviceId { get; set; }
        /// <summary>
        /// device os type
        /// </summary>
        /// <value></value>
        public string DevicePlatform { get; set; }

    }
}