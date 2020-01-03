namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs
{
    internal class NotificationMessageEn : INotificationMessage
    {
        private string Title { get; set; }
        public string Body { get; set; }
        private string sprintName { get; }

        public NotificationMessageEn(string sprintName)
        {
            this.sprintName = sprintName;
        }

        public void FinalCall()
        {
            this.Title = $"Final call for {this.sprintName}";
        }

        public void OndDayBefore()
        {
            this.Title = $"24 hour before {this.sprintName} goes Live";
        }

        public void OneHourBeforeLive()
        {
            this.Title = $"1 hour before {this.sprintName} goes Live";
        }

        public void OnLive()
        {
            this.Title = $"{this.sprintName} is now Live";
        }

        public void Expired()
        {
            this.Title = $"Tou failed to mark attendace for {this.sprintName}";
        }

        public string GetTitle() => this.Title;

        public string GetBody() => this.Body;
    }
}