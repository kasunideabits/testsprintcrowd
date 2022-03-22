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
        HostAllowCount = 500,
        UserAllowCount = 10,
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Host = "Host";
        public const string UnlimitedUser = "UnlimitedUser";
    }
}