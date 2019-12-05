namespace SprintCrowd.BackEnd.Web.ScrowdUser.Models
{
    public class UserSettingsModel
    {
        public ReminderModel Reminder { get; set; }
        public string Language { get; set; }
    }

    public class ReminderModel
    {
        public bool TwentyForH { get; set; }
        public bool OneH { get; set; }
        public bool FiftyM { get; set; }
        public bool EventStart { get; set; }
        public bool FinalCall { get; set; }
    }
}