namespace SprintCrowd.BackEnd.Web.SocialShare
{

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SocialShare;


    /// <summary>
    /// Social media controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SocialShareController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SocialShareController
        /// </summary>
        public SocialShareController(ISocialShareService socialShareService)
        {
            this.socialShareService = socialShareService;
        }

        private ISocialShareService socialShareService { get; }

        /// <summary>
        /// creates Social Shareble link
        /// </summary>
        /// <param name="socialLink">info about the Social media link creation</param>
        [HttpPost("link")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateEvent([FromBody] SocialLink socialLink)
        {
            var result = await this.socialShareService.GetSmartLink(socialLink);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// creates Social Shareble link
        /// </summary>
        /// <param name="socialInvitation">info about the Social media link creation</param>
        [HttpPost("invitation")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateInvitation([FromBody] object customdata)
        {
            var result = await this.socialShareService.updateTokenAndGetInvite(customdata);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }


    }
}