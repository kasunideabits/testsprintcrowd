namespace SprintCrowdBackEnd.Application
{
    /// <summary>
    /// Each and every rest api will return a object of this, it will make it easy for the mobile clients
    /// to do error handling
    /// </summary>
    public class ResponseObject
    {
        public int StatusCode {get; set;}
        public string ErrorDescription {get; set;}
        public dynamic Data {get; set;}
    }
}