namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    /// <summary>
    /// inerface for private event repo
    /// </summary>
    public interface ISprintParticipantRepo
    {
        /// <summary>
        /// adds new private event to database
        /// </summary>
        /// <param name="privateEventCreate">event model</param>
        /// <returns>Created sprint details</returns>
        Task<SprintParticipant> AddSprintParticipant(SprintParticipant privateEventCreate);

        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();
    }
}