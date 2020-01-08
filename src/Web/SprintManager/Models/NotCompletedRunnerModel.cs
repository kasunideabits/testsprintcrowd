namespace SprintCrowd.BackEnd.Web.SprintManager
{
    using System.Collections.Generic;

    public class NotCompletedRunnerModel
    {
        public int SprintId { get; set; }
        public List<NotCompletedRunners> Runners { get; set; }
    }
}