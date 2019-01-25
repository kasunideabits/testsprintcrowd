namespace SprintCrowd.Backend.Application
{
    using System;

    [Serializable]
    public class EmailNotProvidedException : ApplicationException
    {
        public EmailNotProvidedException() : 
            base(ApplicationErrorCodes.EmailNotProvided,
                "Email address is required for first time registration." ) 
        { 

        }
        
        protected EmailNotProvidedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}