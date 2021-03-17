using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Common
{
    /// <summary>
    /// Used for call Gps log api
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
    }
}
