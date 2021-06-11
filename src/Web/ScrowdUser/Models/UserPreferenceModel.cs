namespace SprintCrowd.BackEnd.Web.ScrowdUser.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserPreferenceModel
    {
        [Required]
        public DaysModel Day { get; set; }

        [Required]
        public TimeModel Time { get; set; }

        [Required]
        public DistanceModel Distance { get; set; }
    }

    public class UserEmailModel
    {
        [Required]
        public string Email { get; set; }

    }

    public class DaysModel
    {
        [Required]
        public bool Mon { get; set; }

        [Required]
        public bool Tue { get; set; }

        [Required]
        public bool Wed { get; set; }

        [Required]
        public bool Thur { get; set; }

        [Required]
        public bool Fri { get; set; }

        [Required]
        public bool Sat { get; set; }

        [Required]
        public bool Sun { get; set; }
    }

    public class TimeModel
    {
        [Required]
        public bool Morning { get; set; }

        [Required]
        public bool AfterNoon { get; set; }

        [Required]
        public bool Evening { get; set; }

        [Required]
        public bool Night { get; set; }
    }

    public class DistanceModel
    {
        [Required]
        public bool TwoToFive { get; set; }

        [Required]
        public bool SixToTen { get; set; }

        [Required]
        public bool ElevenToFifteen { get; set; }

        [Required]
        public bool SixteenToTwenty { get; set; }

        [Required]
        public bool TOneToThirty { get; set; }

        [Required]
        public bool ThirtyOneToFortyOne { get; set; }
    }

}