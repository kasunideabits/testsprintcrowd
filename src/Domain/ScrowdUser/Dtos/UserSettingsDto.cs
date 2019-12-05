namespace SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserSettingsDto
    {
        public UserSettingsDto(string lang, UserNotificationReminder userReminder)
        {
            this.Reminder = new ReminderDto(userReminder.TwentyFourH, userReminder.OneH, userReminder.FiftyM, userReminder.EventStart, userReminder.FinalCall);
            this.Language = lang;
        }
        public ReminderDto Reminder { get; }
        public string Language { get; }
    }

    public class ReminderDto
    {
        public ReminderDto(bool twentyFourH, bool oneH, bool fiftyM, bool eventStart, bool finalCall)
        {
            this.TwentyForH = twentyFourH;
            this.OneH = oneH;
            this.FiftyM = fiftyM;
            this.EventStart = eventStart;
            this.FinalCall = finalCall;
        }
        public bool TwentyForH { get; }
        public bool OneH { get; }
        public bool FiftyM { get; }
        public bool EventStart { get; }
        public bool FinalCall { get; }
    }
}