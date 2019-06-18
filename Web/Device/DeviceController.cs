using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Domain.Device;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace backend.Web.Device
{

    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class DeviceController : ControllerBase
    {

        private IDeviceService DeviceService;

        public DeviceController(IDeviceService deviceService)
        {
            this.DeviceService = deviceService;
        }

        [HttpPost]
        [Route("info")]
        public async Task<ResponseObject> SetDeviceInfo([FromBody] AppDownloads AppData)
        {
            AppDownloads appdownloads = await this.DeviceService.SetDeviceInfo(AppData);

            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = appdownloads };
        }

        [HttpGet]
        [Route("info")]
        public async Task<ResponseObject> GetDeviceInfo()
        {
            DeviceModal appdownloads = await this.DeviceService.GetDeviceInfo();

            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = appdownloads };
        }
    }
}