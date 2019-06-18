using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SprintCrowd.BackEnd.Domain.Device;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Device
{
    public class DeviceRepo : IDeviceRepo
    {

        private ScrowdDbContext DbContext;

        public DeviceRepo(ScrowdDbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="DeviceData">device information</param>
        public async Task<AppDownloads> AddDeviceInfo(AppDownloads DeviceData)
        {
            var downloadsInfo = await this.DbContext.AppDownloads.AddAsync(DeviceData);
            return downloadsInfo.Entity;
        }

        public async Task<DeviceModal> GetDeviceInfo()
        {
            var downloadAllCount = this.DbContext.AppDownloads.Count(u => u.DeviceId != null);
            var downloadIosCount = this.DbContext.AppDownloads.Count(u => u.DevicePlatform == "IOS");
            var downloadAndroidCount = this.DbContext.AppDownloads.Count(u => u.DevicePlatform == "ANDROID");
            DeviceModal devices = new DeviceModal(downloadAllCount, downloadIosCount, downloadAndroidCount);

            return devices;
        }

        public async Task<AppDownloads> GetUUID(string UUID)
        {
            AppDownloads uuid = await this.DbContext.AppDownloads.FirstOrDefaultAsync(u => u.DeviceId == UUID);
            return uuid;
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }
    }
}