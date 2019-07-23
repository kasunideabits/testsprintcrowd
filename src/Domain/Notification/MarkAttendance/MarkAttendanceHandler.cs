namespace SprintCrowd.BackEnd.Domain.Notification.MarkAttendance
{
    using System.Threading.Tasks;
    using Coravel.Queuing.Interfaces;
    using SprintCrowd.BackEnd.Infrastructure.Notifier;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Class responsible for send notification which indicating x user mark
    /// attendance for y event
    /// </summary>
    public class MarkAttendanceHandler : IMarkAttendanceHandler
    {
        /// <summary>
        /// Initialize MarkAttendanceHandler class
        /// </summary>
        /// <param name="queue">queue task instance</param>
        /// <param name="notifyFactory">notification factory</param>
        public MarkAttendanceHandler(IQueue queue, INotifyFactory notifyFactory)
        {
            this.Queue = queue;
            this.NotifyFactory = notifyFactory;
        }

        private IQueue Queue { get; }
        private INotifyFactory NotifyFactory { get; }

        /// <summary>
        /// Execute notification task
        /// </summary>
        /// <param name="markAttendance">mark attendance paritiipant details</param>
        /// <returns>task completed or not</returns>
        public Task Execute(MarkAttendance markAttendance)
        {
            this.Queue.QueueTask(() =>
            {
                using(var context = new ScrowdDbFactory().CreateDbContext())
                {
                    var message = new MarkAttendanceNotification(
                        markAttendance.SprintId,
                        markAttendance.UserId,
                        markAttendance.Name,
                        markAttendance.ProfilePicture
                    );
                    this.SendNotification(markAttendance.SprintId, message);
                }
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Send notification message via <see cref="NotifyFactory"/>
        /// </summary>
        /// <param name="sprintId">sprint id for marked attendance</see></param>
        /// <param name="message"><see cref="MarkAttendanceNotification"> notification message </see></param>
        private Task SendNotification(int sprintId, MarkAttendanceNotification message)
        {
            IChannel channel = this.NotifyFactory.CreateChannel(MarkAttendaceHelper.Channels.GetChannel(sprintId));
            channel.Publish(MarkAttendaceHelper.Events.GetEvent(), message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Mark attendance event notification message, define who was marked the attendance
        /// </summary>
        internal class MarkAttendanceNotification
        {
            /// <summary>
            /// Initialize <see cref="MarkAttendanceNotification"/> class
            /// </summary>
            /// <param name="sprintId">sprint id</param>
            /// <param name="userId">user id</param>
            /// <param name="name">name for user</param>
            /// <param name="profilePicture">url for user's profile picture</param>
            public MarkAttendanceNotification(int sprintId, int userId, string name, string profilePicture)
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
}