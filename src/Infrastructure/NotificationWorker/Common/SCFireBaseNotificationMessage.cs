using Newtonsoft.Json.Linq;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    internal class SCFireBaseNotificationMessage
    {
        public SCFireBaseNotificationMessage(JToken token)
        {
            this.Title = (string)token ["title"];
            this.Body = (string)token ["body"];
        }

        public string Title { get; }
        public string Body { get; }
    }
}