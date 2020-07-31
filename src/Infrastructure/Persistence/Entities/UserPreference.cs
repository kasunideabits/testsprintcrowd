namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class UserPreference : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Mon { get; set; } = true;
        public bool Tue { get; set; } = true;
        public bool Wed { get; set; } = true;
        public bool Thur { get; set; } = true;
        public bool Fri { get; set; } = true;
        public bool Sat { get; set; } = true;
        public bool Sun { get; set; } = true;
        public bool Morning { get; set; } = true;
        public bool AfterNoon { get; set; } = true;
        public bool Evening { get; set; } = true;
        public bool Night { get; set; } = true;
        public bool TwoToFive { get; set; } = true;
        public bool SixToTen { get; set; } = true;
        public bool ElevenToFifteen { get; set; } = true;
        public bool SixteenToTwenty { get; set; } = true;
        public bool TOneToThirty { get; set; } = true;
        public bool ThirtyOneToFortyOne { get; set; } = true;
        public virtual User User { get; set; }
    }
}