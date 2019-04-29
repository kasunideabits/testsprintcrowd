namespace SprintCrowd.Backend.Application
{
    using System;
    using SprintCrowdBackEnd.Application;

    /// <summary>
    /// Custom exception class for use application wide.
    /// </summary>
    [Serializable]
    public class ApplicationException : Exception
    {
        /// <summary>
        /// Gets application error code.
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// default constrcutor.
        /// </summary>
        public ApplicationException() { }

        /// <summary>
        /// constrcutor with message.
        /// <param name="message">exception message</param>
        /// </summary>
        public ApplicationException(string message) : base(message)
        {
            this.ErrorCode = (int)ApplicationErrorCode.UnknownError;
        }

        /// <summary>
        /// constrcutor with message.
        /// <param name="errorCode">exception message</param>
        /// <param name="message">exception message</param>
        /// </summary>
        public ApplicationException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// constrcutor with message.
        /// <param name="message">exception message</param>
        /// <param name="inner">exception message</param>
        /// </summary>
        public ApplicationException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// constrcutor with message.
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// </summary>
        protected ApplicationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}