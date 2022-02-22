namespace SprintCrowd.BackEnd.Application
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
        public dynamic ErrorDescription { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>data that will be returned.</value>
        public dynamic Data { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <totalItems>total items if have pages</value>
        public int totalItems { get; set; }
    }

    public class ProgramDashboardResponseObject
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
        public dynamic ErrorDescription { get; set; }
        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>data that will be returned.</value>
        public dynamic Data { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <totalItems>total Events</value>
        public int totalEvents { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <totalItems>total Participants</value>
        public int totalParticipants { get; set; }
    }
}