namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class UserPreference : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Mon { get; set; } = true;
        public bool Tue { get; set; } = false;
        public bool Wed { get; set; } = false;
        public bool Thur { get; set; } = true;
        public bool Fri { get; set; } = false;
        public bool Sat { get; set; } = true;
        public bool Sun { get; set; } = false;
        public bool Morning { get; set; } = true;
        public bool AfterNoon { get; set; } = false;
        public bool Evening { get; set; } = true;
        public bool Night { get; set; } = false;
        public bool TwoToTen { get; set; } = true;
        public bool EleToTwenty { get; set; } = false;
        public bool TOneToThirty { get; set; } = false;
        public virtual User User { get; set; }
    }
}