namespace SprintCrowd.BackEnd.Application
{
    /// <summary>
    /// User type used for various things, ex: sending registeration request to identity server.
    /// </summary>
    public enum UserType
    {

        /// <summary>
        /// User type facebook.
        /// </summary>
        Facebook = 1,
        /// <summary>
        /// User type google.
        /// </summary>
        Google = 2,
        /// <summary>
        /// normal user, without any third party provider.
        /// </summary>
        NormalUser = 3,
        /// <summary>
        /// admin user, have access to Control panel.
        /// </summary>
        AdminUser = 4,
        /// <summary>
        /// Sys Admin User Type
        /// </summary>
        SystemUser = 5,

        /// <summary>
        /// Apple User ype
        /// </summary>
        AppleUser = 6,

        /// <summary>
        /// Email User
        /// </summary>
        EmailUser = 7

    }

}