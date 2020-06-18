namespace SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserPreferenceDto
    {
        public UserPreferenceDto(UserPreference userPreference)
        {
            this.Day = new DaysDto(userPreference.Mon, userPreference.Tue, userPreference.Wed, userPreference.Thur, userPreference.Fri, userPreference.Sat, userPreference.Sun);
            this.Time = new TimeDto(userPreference.Morning, userPreference.AfterNoon, userPreference.Evening, userPreference.Night);
            this.Distance = new DistanceDto(userPreference.TwoToFive, userPreference.SixToTen, userPreference.ElevenToFifteen, userPreference.SixteenToTwenty, userPreference.TOneToThirty, userPreference.ThirtyOneToFortyOne);
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
        public DistanceDto(bool twoToFive, bool sixToTen, bool elevenToFifteen, bool sixteenToTwenty, bool tOneToThirty, bool thirtyOneToFortyOne)
        {
            this.TwoToFive = twoToFive;
            this.SixToTen = sixToTen;
            this.ElevenToFifteen = elevenToFifteen;
            this.SixteenToTwenty = sixteenToTwenty;
            this.TOneToThirty = tOneToThirty;
            this.ThirtyOneToFortyOne = thirtyOneToFortyOne;
        }
        public bool TwoToFive { get; }
        public bool SixToTen { get; }
        public bool ElevenToFifteen { get; }
        public bool SixteenToTwenty { get; }
        public bool TOneToThirty { get; }
        public bool ThirtyOneToFortyOne { get; }
    }

}