namespace SprintCrowd.BackEnd.Domain.Device
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    /// <summary>
    /// Device Repo
    /// </summary>
    public class DeviceRepo : IDeviceRepo
    {
        private ScrowdDbContext dbContext;
        /// <summary>
        /// Initialize dbcontext instance
        /// </summary>
        public DeviceRepo(ScrowdDbContext dbContxt)
        {
            this.dbContext = dbContxt;
        }

        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="deviceData">device information</param>
        public async Task<AppDownloads> AddDeviceInfo(AppDownloads deviceData)
        {
            var downloadsInfo = await this.dbContext.AppDownloads.AddAsync(deviceData);
            return downloadsInfo.Entity;
        }

        /// <summary>
        /// Get app downloads devices info
        /// </summary>
        public async Task<DeviceModal> GetDeviceInfo()
        {
            var downloadAllCount = await this.dbContext.AppDownloads.CountAsync(u => u.DeviceId != null);
            var downloadIosCount = await this.dbContext.AppDownloads.CountAsync(u => u.DevicePlatform == "IOS");
            var downloadAndroidCount = this.dbContext.AppDownloads.Count(u => u.DevicePlatform == "ANDROID");
            DeviceModal devices = new DeviceModal(downloadAllCount, downloadIosCount, downloadAndroidCount);

            return devices;
        }

        /// <summary>
        /// Get device uuid of the app
        /// </summary>
        public async Task<AppDownloads> GetUUID(string uuid)
        {
            return await this.dbContext.AppDownloads.FirstOrDefaultAsync(u => u.DeviceId == uuid);
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public async void SaveChanges()
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}