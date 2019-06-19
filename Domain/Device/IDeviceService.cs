namespace SprintCrowd.BackEnd.Domain.Device
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Device Interface
    /// </summary>
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
        Task<DeviceModal> GetDeviceInfo();
    }
}