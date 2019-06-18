using System.Collections.Generic;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Device
{
    public interface IDeviceRepo
    {
        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="DeviceData">device information</param>
        Task<AppDownloads> AddDeviceInfo(AppDownloads DeviceData);
        /// <summary>
        /// Get device uuid and os
        /// </summary>
        /// <param name="DeviceData">device information</param>
        Task<DeviceModal> GetDeviceInfo();
        /// <summary>
        /// Get device uuid
        /// </summary>
        /// <param name="UUID">application UUID</param>
        Task<AppDownloads> GetUUID(string UUID);
        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();
    }
}