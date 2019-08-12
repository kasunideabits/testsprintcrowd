namespace SprintCrowd.BackEnd.Domain.Device
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    /// <summary>
    /// Device service
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private IDeviceRepo deviceRepo;

        /// <summary>
        /// Initialize device instance of device service
        /// </summary>
        public DeviceService(IDeviceRepo dRepo)
        {
            this.deviceRepo = dRepo;
        }

        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="appData">device information</param>
        public async Task<AppDownloads> SetDeviceInfo(AppDownloads appData)
        {
            AppDownloads downloadInfo = new AppDownloads();
            downloadInfo.DeviceId = appData.DeviceId;
            downloadInfo.DevicePlatform = appData.DevicePlatform.ToUpperInvariant();

            AppDownloads checkUUID = await this.deviceRepo.GetUUID(appData.DeviceId);

            if (checkUUID == null)
            {
                AppDownloads appDownload = await this.deviceRepo.AddDeviceInfo(downloadInfo);

                if (appDownload != null)
                {
                    await this.deviceRepo.SaveChanges();
                }

                return appDownload;
            }

            return null;
        }

        /// <summary>
        /// retrives downloads count per os
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceModal> GetDeviceInfo()
        {
            return await this.deviceRepo.GetDeviceInfo();
        }

    }
}