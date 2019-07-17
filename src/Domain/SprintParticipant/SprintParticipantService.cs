namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Notifier;

    /// <summary>
    /// Implements ISprintParticipantService interface for hanle sprint participants
    /// </summary>
    public class SprintParticipantService : ISprintParticipantService
    {
        /// <summary>
        /// Initalize SprintParticipantService class
        /// </summary>
        /// <param name="sprintParticipantRepo">sprint participant repository</param>
        /// <param name="notifyFactory">notifi factory</param>
        public SprintParticipantService(ISprintParticipantRepo sprintParticipantRepo, INotifyFactory notifyFactory)
        {
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.NotifyFactory = notifyFactory;
        }

        private ISprintParticipantRepo SprintParticipantRepo { get; }

        private INotifyFactory NotifyFactory { get; }

        /// <summary>
        /// Mark the attendece for the given sprint and notify with evnet
        /// EventName.MarkedAttenence with MarkAttendenceMessage message
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for for participant</param>
        public async Task MarkAttendence(int sprintId, int userId)
        {
            var result = await this.SprintParticipantRepo.MarkAttendence(sprintId, userId);
            string channelName = Channels.GetChannel(sprintId);
            IChannel channel = this.NotifyFactory.CreateChannel(channelName);
            var msg = new MarkAttendanceMessage(sprintId, userId, result.Name, result.ProfilePicture);
            await channel.Publish(EventName.MarkedAttenence, msg);
            this.SprintParticipantRepo.SaveChanges();
            return;
        }
    }

    /// <summary>
    /// Mark attendance message for publish with sccessfuly update
    /// </summary>
    internal class MarkAttendanceMessage
    {

        /// <summary>
        /// Initalize MarkAttendanceMessage class
        /// </summary>
        /// <param name="sprintId">marked sprint id</param>
        /// <param name="userId">marked user id</param>
        /// <param name="name">name for user</param>
        /// <param name="profilePicture">uri for user profile picture</param>
        public MarkAttendanceMessage(int sprintId, int userId, string name, string profilePicture)
        {
            this.SprintId = sprintId;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
        }

        /// <summary>
        /// Gets marked sprint id
        /// </summary>
        public int SprintId { get; }

        /// <summary>
        /// Gets marked user id
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets name for user
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets uri for user profile picture
        /// </summary>
        public string ProfilePicture { get; }
    }
}