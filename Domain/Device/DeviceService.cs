using System.Collections.Generic;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Device
{
    public class DeviceService : IDeviceService
    {
        private IDeviceRepo DeviceRepo;

        public DeviceService(IDeviceRepo deviceRepo)
        {
            this.DeviceRepo = deviceRepo;
        }

        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="appData">device information</param>
        public async Task<AppDownloads> SetDeviceInfo(AppDownloads appData)
        {
            AppDownloads DownloadInfo = new AppDownloads();
            DownloadInfo.DeviceId = appData.DeviceId;
            DownloadInfo.DevicePlatform = appData.DevicePlatform.ToUpperInvariant();

            AppDownloads checkUUID = await this.DeviceRepo.GetUUID(appData.DeviceId);

            if (checkUUID == null)
            {
                AppDownloads AppDownload = await this.DeviceRepo.AddDeviceInfo(DownloadInfo);

                if (AppDownload != null)
                {
                    this.DeviceRepo.SaveChanges();
                }

                return AppDownload;
            }
            return null;

        }

        Task<DeviceModal> IDeviceService.GetDeviceInfo()
        {
            return this.DeviceRepo.GetDeviceInfo();
        }

    }
}