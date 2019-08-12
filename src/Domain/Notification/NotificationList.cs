namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System.Collections.Generic;

    /// <summary>
    /// Class hold all notificaiton related to user
    /// </summary>
    public class NotificationList
    {
        /// <summary>
        /// Notificaiton list related to user
        /// </summary>
        /// <returns>notificaiton list</returns>
        public List<dynamic> Notifications { get; set; } = new List<dynamic>();
    }
}