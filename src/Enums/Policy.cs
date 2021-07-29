using System.ComponentModel;

namespace SprintCrowd.BackEnd.Enums
{
    public static class Policy
    {
        public const string ADMIN = "ADMIN";
        public const string USER = "USER";
        public const string HOST = "HOST";
    }

    public enum AllowCrowdSprints
    {
        HostAllowCount = 50,
        UserAllowCount = 3,
    }
}