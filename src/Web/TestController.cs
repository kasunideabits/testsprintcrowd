namespace SprintCrowd.BackEnd.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// just a test controller used for quick code testing.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    //   [Authorize]
    public class TestController : ControllerBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>

        /// <summary>
        /// just a testing endpoint used for quick code testing
        /// </summary>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string [] { "value1", "value2", "value3", "value4", "value5", "value6" };
        }
    }
}