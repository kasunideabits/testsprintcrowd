namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;


    /// <summary>
    /// Private Event repositiory
    /// </summary>
    public class SprintParticipantRepo : ISprintParticipantRepo
    {
        private ScrowdDbContext dbContext;
        /// <summary>
        /// intializes an instace of EventRepo
        /// </summary>
        /// <param name="dbContext">db context</param>
        public SprintParticipantRepo(ScrowdDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="privateEventCreate">event model</param>
        /// <returns>added private sprint result</returns>
        public async Task<SprintParticipant> AddSprintParticipant(SprintParticipant privateEventCreate)
        {
            var result = await this.dbContext.SprintParticipant.AddAsync(privateEventCreate);
            return result.Entity;
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