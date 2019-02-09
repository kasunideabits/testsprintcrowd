namespace SprintCrowdBackEnd.Application
{
    /// <summary>
    /// Each and every rest api will return a object of this, it will make it easy for the mobile clients
    /// to do error handling.
    /// </summary>
    public class ResponseObject
    {
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>status code for the operation.</value>
        public int StatusCode { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>if error then the error description.</value>
        public string ErrorDescription { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>data that will be returned.</value>
        public dynamic Data { get; set; }
    }
}