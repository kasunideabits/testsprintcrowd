namespace SprintCrowd.Backend.Web
{
    /// <summary>
    /// Represents an error returned by the API.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets the application specific error code.
        /// </summary>
        /// <value>The error code.</value>
        public int ErrorCode { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorResponse" />.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The error message.</param>
        public ErrorResponse(int errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
        }
    }
}