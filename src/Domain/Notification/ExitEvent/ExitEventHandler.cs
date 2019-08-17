namespace SprintCrowd.BackEnd.Domain.Notification.ExitEvent
{
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
    /// <param name="notifyFactory">notification factory</param>
    public ExitEventHandler(INotifyFactory notifyFactory)
    {
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
      using (var context = new ScrowdDbFactory().CreateDbContext())
      {
        var message = new ExitNotification(
            exitEvent.UserId,
            exitEvent.Name,
            exitEvent.ProfilePicture,
            exitEvent.SprintName);
        this.SendNotification(exitEvent.SprintId, message);
      }
      return Task.CompletedTask;
    }
    /// <summary>
    /// Send notification message via <see cref="NotifyFactory"/>
    /// </summary>
    /// <param name="sprintId">sprint id</param>
    /// <param name="message"><see cref="ExitNotification"> notification message </see></param>
    private Task SendNotification(int sprintId, ExitNotification message)
    {
      IChannel sprintChannel = this.NotifyFactory.CreateChannel(ChannelNames.Sprint(sprintId));
      sprintChannel.Publish(EventNames.GetExitSprintEvent(), message);
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