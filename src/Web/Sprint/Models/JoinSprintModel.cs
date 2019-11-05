using System.ComponentModel.DataAnnotations;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Web.Event
{
    /// <summary>
    /// model for holding join event data
    /// </summary>
    public class JoinSprintModel
    {
        /// <summary>
        /// Sprint Id
        /// </summary>
        /// <value>Sprint Id</value>
        public int SprintId { get; set; }

        public int NotificationId { get; set; }

        public bool Status { get; set; }
    }
}