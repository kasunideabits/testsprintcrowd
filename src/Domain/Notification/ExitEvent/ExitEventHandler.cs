namespace SprintCrowd.BackEnd.Domain.Notification.ExitEvent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Coravel.Queuing.Interfaces;
    using SprintCrowd.BackEnd.Infrastructure.Notifier;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Class responsible for send notification which indicating x user exit
    /// form y event.
    /// </summary>
    public class ExitEventHandler : IExitEventHandler
    {
        /// <summary>
        /// Initialize ExitEventHandler class
        /// </summary>
        /// <param name="queue">queue task instance</param>
        /// <param name="notifyFactory">notification factory</param>
        public ExitEventHandler(IQueue queue, INotifyFactory notifyFactory)
        {
            this.Queue = queue;
            this.NotifyFactory = notifyFactory;
        }

        private IQueue Queue { get; }
        private INotifyFactory NotifyFactory { get; }

        /// <summary>
        /// Execute notification task
        /// </summary>
        /// <param name="exitEvent">exit event paritiipant details</param>
        /// <returns>task completed or not</returns>
        public Task Execute(ExitEvent exitEvent)
        {
            this.Queue.QueueTask(() =>
            {
                using(var context = new ScrowdDbFactory().CreateDbContext())
                {
                    List<int> users = this.GetUsersToNotify(context, exitEvent.SprintId, exitEvent.UserId);
                    var message = new ExitNotification(
                        exitEvent.UserId,
                        exitEvent.Name,
                        exitEvent.ProfilePicture,
                        exitEvent.SprintName);
                    this.SendNotification(exitEvent.SprintId, users, message);
                }

            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get users which want to send notificaiton
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="sprintId">related sprint id</param>
        /// <param name="userId">who exit for the event</param>
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
        /// <param name="sprintId">sprint id</param>
        /// <param name="users">user id's for send notifications</param>
        /// <param name="message"><see cref="ExitNotification"> notification message </see></param>
        private Task SendNotification(int sprintId, List<int> users, ExitNotification message)
        {
            IChannel channel = this.NotifyFactory.CreateChannel(ExitEventHelper.Channels.ExitUser());
            users.ForEach(uid =>
            {
                channel.Publish(ExitEventHelper.Events.GetEvent(uid), message);
            });
            IChannel sprintChannel = this.NotifyFactory.CreateChannel(ExitEventHelper.Channels.ExitSprint(sprintId));
            channel.Publish(ExitEventHelper.Events.GetSprintEvent(), message);
            return Task.CompletedTask;
        }

        internal class ExitNotification
        {
            /// <summary>
            /// Initialize ExitNotification class
            /// </summary>
            /// <param name="userId">user id for who has exited</param>
            /// <param name="name">name for who has exited</param>
            /// <param name="profilePicture">profile picture url for user who has exited</param>
            /// <param name="sprintName">sprint name</param>
            public ExitNotification(int userId, string name, string profilePicture, string sprintName)
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