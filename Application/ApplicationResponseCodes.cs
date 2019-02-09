namespace SprintCrowd.Backend.Application
{
    /// <summary>
    /// response codes to use when responding to rest requests.
    /// </summary>
    public enum ApplicationResponseCodes
    {
        /// <summary>
        /// All went well.
        /// </summary>
        Success = 200,
        /// <summary>
        /// already exists.
        /// </summary>
        Exists = 409,
        /// <summary>
        /// expired.
        /// </summary>
        Expired = 401,
    }
}