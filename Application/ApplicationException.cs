namespace SprintCrowd.Backend.Application
{
    using System;
    using SprintCrowdBackEnd.Application;

    [Serializable]
    public class ApplicationException : Exception
    {
        public int ErrorCode { get;}
        public ApplicationException() { }
        public ApplicationException(string message) : base(message)
        {
            this.ErrorCode = (int)ApplicationErrorCodes.UnknownError;
        }

        public ApplicationException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }
        public ApplicationException(string message, System.Exception inner) : base(message, inner) { }
        protected ApplicationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}