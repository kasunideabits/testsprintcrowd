using System.ComponentModel.DataAnnotations;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// model for holding download device info
    /// </summary>
    /// <value></value>
    public class AppDownloads : BaseEntity
    {
        /// <summary>
        /// AppDownload id
        /// </summary>
        /// <value>Unique Id</value>
        public int Id { get; set; }
        /// <summary>
        /// uuid of the device
        /// </summary>
        /// <value>device id</value>
        [MaxLength(50)]
        public string DeviceId { get; set; }
        /// <summary>
        /// device os type
        /// </summary>
        /// <value>platfrom ios or android</value>
        [MaxLength(10)]
        public string DevicePlatform { get; set; }
    }
}