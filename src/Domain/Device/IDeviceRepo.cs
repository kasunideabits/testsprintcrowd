namespace SprintCrowd.BackEnd.Domain.Device
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    /// <summary>
    /// Device Repo
    /// </summary>
    public interface IDeviceRepo
    {
        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="deviceData">device information</param>
        Task<AppDownloads> AddDeviceInfo(AppDownloads deviceData);
        /// <summary>
        /// Get device uuid and os
        /// </summary>
        Task<DeviceModal> GetDeviceInfo();
        /// <summary>
        /// Get device uuid
        /// </summary>
        /// <param name="uuid">application UUID</param>
        Task<AppDownloads> GetUUID(string uuid);
        /// <summary>
        /// saves changed to db
        /// </summary>
        Task SaveChanges();
    }
}