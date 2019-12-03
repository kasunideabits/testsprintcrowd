namespace SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserPreferenceDto
    {
        public UserPreferenceDto(UserPreference userPreference)
        {
            this.Day = new DaysDto(userPreference.Mon, userPreference.Tue, userPreference.Wed, userPreference.Thur, userPreference.Fri, userPreference.Sat, userPreference.Sun);
            this.Time = new TimeDto(userPreference.Morning, userPreference.AfterNoon, userPreference.Evening, userPreference.Night);
            this.Distance = new DistanceDto(userPreference.TwoToTen, userPreference.EleToTwenty, userPreference.TOneToThirty);
        }
        public DaysDto Day { get; set; }
        public TimeDto Time { get; set; }
        public DistanceDto Distance { get; set; }
    }

    public class DaysDto
    {
        public DaysDto(bool mon, bool tue, bool wed, bool thur, bool fri, bool sat, bool sun)
        {
            this.Mon = mon;
            this.Tue = tue;
            this.Wed = wed;
            this.Thur = thur;
            this.Fri = fri;
            this.Sat = sat;
            this.Sun = sun;
        }
        public bool Mon { get; }
        public bool Tue { get; }
        public bool Wed { get; }
        public bool Thur { get; }
        public bool Fri { get; }
        public bool Sat { get; }
        public bool Sun { get; }
    }

    public class TimeDto
    {
        public TimeDto(bool morning, bool afternoon, bool evening, bool night)
        {
            this.Morning = morning;
            this.AfterNoon = afternoon;
            this.Evening = evening;
            this.Night = night;
        }
        public bool Morning { get; }
        public bool AfterNoon { get; }
        public bool Evening { get; }
        public bool Night { get; }
    }

    public class DistanceDto
    {
        public DistanceDto(bool twoToTen, bool eleToTwenty, bool tOneToThirty)
        {
            this.TwoToTen = twoToTen;
            this.EleToTwenty = eleToTwenty;
            this.TOneToThirty = tOneToThirty;
        }
        public bool TwoToTen { get; }
        public bool EleToTwenty { get; }
        public bool TOneToThirty { get; }
    }

}