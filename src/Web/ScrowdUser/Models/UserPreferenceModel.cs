namespace SprintCrowd.BackEnd.Web.ScrowdUser.Models
{
    public class UserPreferenceModel
    {
        public DaysModel Day { get; set; }
        public TimeModel Time { get; set; }
        public DistanceModel Distance { get; set; }
    }

    public class DaysModel
    {
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thur { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
        public bool Sun { get; set; }
    }

    public class TimeModel
    {
        public bool Morning { get; set; }
        public bool AfterNoon { get; set; }
        public bool Evening { get; set; }
        public bool Night { get; set; }
    }

    public class DistanceModel
    {
        public bool TwoToTen { get; set; }
        public bool EleToTwenty { get; set; }
        public bool TOneToThirty { get; set; }
    }

}