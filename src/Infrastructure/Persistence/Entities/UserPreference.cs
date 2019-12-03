namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class UserPreference : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Mon { get; set; } = false;
        public bool Tue { get; set; } = false;
        public bool Wed { get; set; } = false;
        public bool Thur { get; set; } = false;
        public bool Fri { get; set; } = false;
        public bool Sat { get; set; } = false;
        public bool Sun { get; set; } = false;
        public bool Morning { get; set; } = false;
        public bool AfterNoon { get; set; } = false;
        public bool Evening { get; set; } = false;
        public bool Night { get; set; } = false;
        public bool TwoToTen { get; set; } = false;
        public bool EleToTwenty { get; set; } = false;
        public bool TOneToThirty { get; set; } = false;
        public virtual User User { get; set; }
    }
}