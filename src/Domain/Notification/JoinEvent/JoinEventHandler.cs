namespace SprintCrowd.BackEnd.Domain.Notification.JoinEvent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Coravel.Queuing.Interfaces;
    using SprintCrowd.BackEnd.Infrastructure.Notifier;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Class responsible for send notification which indicating x user join
    /// for y event to all participant and mange notification datastore
    /// </summary>
    // TODO: store notifications
    public class JoinEventHandler : IJoinEventHandler
    {
        /// <summary>
        /// Initialize JoinEventHandler class
        /// </summary>
        /// <param name="queue">queue task instance</param>
        /// <param name="notifyFactory">notification factory</param>
        public JoinEventHandler(IQueue queue, INotifyFactory notifyFactory)
        {
            this.Queue = queue;
            this.NotifyFactory = notifyFactory;
        }

        private IQueue Queue { get; }

        private INotifyFactory NotifyFactory { get; }

        /// <summary>
        /// Execute notification task
        /// </summary>
        /// <param name="joinEvent">join event paritiipant details</param>
        /// <returns>task completed or not</returns>
        public Task Execute(JoinEvent joinEvent)
        {
            this.Queue.QueueTask(() =>
            {
                using(var context = new ScrowdDbFactory().CreateDbContext())
                {
                    List<int> users = this.GetUsersToNotify(context, joinEvent.SprintId, joinEvent.UserId);
                    var message = new JoinedNotification(
                        joinEvent.UserId,
                        joinEvent.Name,
                        joinEvent.ProfilePicture,
                        joinEvent.SprintName);
                    this.SendNotification(users, message);
                }
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get users which want to send notificaiton
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="sprintId">related sprint id</param>
        /// <param name="userId">who joined for the event</param>
        /// <returns>user ids</returns>
        private List<int> GetUsersToNotify(ScrowdDbContext context, int sprintId, int userId)
        {
            return context.SprintParticipant
                .Where(s => s.Sprint.Id == sprintId && s.User.Id != userId)
                .Select(s => s.User.Id)
                .ToList();
        }

        /// <summary>
        /// Send notification message via <see cref="NotifyFactory"/>
        /// </summary>
        /// <param name="users">user id's for send notifications</param>
        /// <param name="message"><see cref="JoinedNotification"> notification message </see></param>
        private Task SendNotification(List<int> users, JoinedNotification message)
        {
            IChannel channel = this.NotifyFactory.CreateChannel(ChannelNames.Join);
            users.ForEach(uid =>
            {
                channel.Publish(EventNames.GetEvent(uid), message);
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Join event notification message, define how as join to an event
        /// </summary>
        internal class JoinedNotification
        {
            /// <summary>
            /// Initialize JoinedNotification class
            /// </summary>
            /// <param name="userId">user id for who has joined</param>
            /// <param name="name">name for who has joined</param>
            /// <param name="profilePicture">profile picture url for user who has joined</param>
            /// <param name="sprintName">sprint name</param>
            public JoinedNotification(int userId, string name, string profilePicture, string sprintName)
            {
                this.UserId = userId;
                this.Name = name;
                this.ProfilePicture = profilePicture;
                this.SprintName = sprintName;
            }

            /// <summary>
            /// Gets users id
            /// </summary>
            public int UserId { get; private set; }

            /// <summary>
            /// Gets user's name
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets url for profile picture
            /// </summary>
            public string ProfilePicture { get; private set; }

            /// <summary>
            /// Gets sprint name
            /// </summary>
            public string SprintName { get; private set; }
        }
    }
}