namespace SprintCrowd.BackEnd.Web.Video
{

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Video;



    /// <summary>
    /// Social media controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class VimeoController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SocialShareController
        /// </summary>
        public VimeoController(IVimeoUploadService vimeoUploadService)
        {
            this.vimeoUploadService = vimeoUploadService;
        }

        private IVimeoUploadService vimeoUploadService { get; }

        /// <summary>
        /// creates Social Vimeo upload link
        /// </summary>
        /// <param name="video-upload-link">info about the vimeo upload link creation</param>
        [HttpPost("video-upload-link")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateEvent([FromBody] VideoUploadParams options)
        {
            var result = await this.vimeoUploadService.GetVimeoUploadLink(options.fileSize);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

    }
}