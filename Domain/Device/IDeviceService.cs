using System.Collections.Generic;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Device
{
    public interface IDeviceService
    {

        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="appData">device information</param>
        Task<AppDownloads> SetDeviceInfo(AppDownloads appData);

        /// <summary>
        /// get device uuid and os
        /// </summary>
        /// <param name="appData">device information</param>
        Task<DeviceModal> GetDeviceInfo();
    }
}