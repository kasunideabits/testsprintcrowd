namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs
{
    internal interface INotificationMessage
    {
        void FinalCall();
        void OndDayBefore();
        void OneHourBeforeLive();
        void OnLive();
        void Expired();
        string GetTitle();
        string GetBody();
    }
}