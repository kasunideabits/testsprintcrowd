namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Event repositiory
    /// </summary>
    public class SprintRepo : ISprintRepo
    {
        private ScrowdDbContext dbContext;
        /// <summary>
        /// intializes an instace of EventRepo
        /// </summary>
        /// <param name="dbContext">db context</param>
        public SprintRepo(ScrowdDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>Available events</returns>
        public async Task<List<Sprint>> GetAllEvents()
        {
            return this.dbContext.Sprint.ToList();
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="SprintToAdd">event model</param>
        /// <returns></returns>
        public async Task<Sprint> AddSprint(Sprint SprintToAdd)
        {
            var result = await this.dbContext.Sprint.AddAsync(SprintToAdd);
            return result.Entity;
        }
        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="SprintData">sprint repository</param>
        public async Task<Sprint> UpdateSprint(Sprint SprintData)
        {
            var result = this.dbContext.Sprint.Update(SprintData);
            return result.Entity;

        }
        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="SprintID">sprint repository</param>
        public async Task<Sprint> GetSprint(int SprintID)
        {
            Sprint sprint = await this.dbContext.Sprint.FindAsync(SprintID);

            return sprint;
        }
        /// <summary>
        /// Save device uuid and os
        /// </summary>
        /// <param name="DeviceData">device information</param>
        public async Task<AppDownloads> AddDeviceInfo(AppDownloads DeviceData)
        {
            var downloadsInfo = await this.dbContext.AppDownloads.AddAsync(DeviceData);
            return downloadsInfo.Entity;
        }
        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

    }
}