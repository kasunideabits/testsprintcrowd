namespace SprintCrowd.Backend.Application
{
    public static class ApplicationErrorCodes
    {
        public const int UnknownError = 99999;

        public const int ExternalLoginError = 10000;
        public const int EmailNotProvided = ExternalLoginError + 100;
        
    }
}