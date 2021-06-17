namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;
    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    // [Authorize]
    public class AblyController : ControllerBase
    {
        /// <summary>
        /// </summary>
        /// <param name="ablyConnectionFactory">ably connection</param>
        public AblyController(IAblyConnectionFactory ablyConnectionFactory)
        {
            this.AblyConnectionFactory = ablyConnectionFactory;
        }

        private IAblyConnectionFactory AblyConnectionFactory { get; }


        /// <summary>
        /// creates Token
        /// </summary>
        [HttpGet("subcribe")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<object> CreateEvent()
        {
            var res = this.AblyConnectionFactory.getSubcribeTokenRequest();
            JObject json = JObject.Parse(res);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = json,
            };
            return json;
        }


    }
}