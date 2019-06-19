namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Application;
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
        public async Task<List<Sprint>> GetAllEvents(int eventType)
        {
            return await this.dbContext.Sprint.Where(s => s.Type == eventType).ToListAsync();
        }

        /// <summary>
        /// Get all ongoing sprint sprints
        /// </summary>
        /// <returns>All ongoing sprints which not completed 24H</returns>
        public async Task<List<Sprint>> GetLiveSprints()
        {
            DateTime now = DateTime.UtcNow;
            DateTime end = now.AddDays(1);
            return await this.dbContext.Sprint
                .Where(s => s.StartDateTime <= DateTime.UtcNow && s.StartDateTime < end)
                .ToListAsync();
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="sprintToAdd">event model</param>
        /// <returns>added sprint result</returns>
        public async Task<Sprint> AddSprint(Sprint sprintToAdd)
        {
            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            return result.Entity;
        }

        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="sprintData">sprint repository</param>
        public async Task<Sprint> UpdateSprint(Sprint sprintData)
        {
            var result = this.dbContext.Sprint.Update(sprintData);
            return result.Entity;
        }

        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="sprintID">sprint repository</param>
        public async Task<Sprint> GetSprint(int sprintID)
        {
            Sprint sprint = await this.dbContext.Sprint.FindAsync(sprintID);

            return sprint;
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