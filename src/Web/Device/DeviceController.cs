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
        [HttpPost]
        [Route("info")]
        public async Task<ResponseObject> SetDeviceInfo([FromBody] AppDownloads AppData)
        {
            AppDownloads appdownloads = await this.DeviceService.SetDeviceInfo(AppData);

            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = appdownloads };
        }
        /// <summary>
        /// get app downloads devices count
        /// </summary>
        [HttpGet]
        [Route("info")]
        public async Task<ResponseObject> GetDeviceInfo()
        {
            DeviceModal appdownloads = await this.DeviceService.GetDeviceInfo();

            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = appdownloads };
        }
    }
}