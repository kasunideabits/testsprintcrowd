namespace SprintCrowd.BackEnd.Web.Device
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Device;
    using SprintCrowd.BackEnd.Enums;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// device controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy.ADMIN)]

    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// Initialize device service instance.
        /// </summary>
        public DeviceController(IDeviceService deviceService)
        {
            this.DeviceService = deviceService;
        }

        private IDeviceService DeviceService { get; }

        /// <summary>
        /// save device info uuid and platform
        /// </summary>
        [HttpPost("info")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> SetDeviceInfo([FromBody] AppDownloads AppData)
        {
            AppDownloads appdownloads = await this.DeviceService.SetDeviceInfo(AppData);

            return this.Ok(new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = appdownloads });
        }
        /// <summary>
        /// get app downloads devices count
        /// </summary>
        [HttpGet("info")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetDeviceInfo()
        {
            DeviceModal appdownloads = await this.DeviceService.GetDeviceInfo();

            return this.Ok(new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = appdownloads });
        }
    }
}