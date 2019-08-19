namespace SprintCrowd.BackEnd.Application
{
    /// <summary>
    /// Application exception error codes.
    /// </summary>
    public enum ApplicationErrorCode
    {
        /// <summary>
        /// Unknown error code, the error is not known
        /// try to minimize the use of this
        /// </summary>
        UnknownError = -1,
        /// <summary>
        /// Internal server error occured
        /// </summary>
        InternalError = 500,

        BadRequest = 400
    }
}