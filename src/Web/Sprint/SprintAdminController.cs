namespace SprintCrowd.BackEnd.Web.Event
{
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Admin.Dashboard;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Enums;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Domain.Device;
    using OfficeOpenXml;
    using System.IO;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using System.Collections.Generic;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;

    /// <summary>
    /// event controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy.ADMIN)]
    public class SprintAdminController : ControllerBase
    {
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="sprintService">sprint service</param>
        /// <param name="userService">user service</param>
        public SprintAdminController(ISprintService sprintService, IUserService userService, IDashboardService dashboardService, IDeviceService deviceService, ISprintParticipantService sprintParticipantService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
            this.DashboardService = dashboardService;
            this.DeviceService = deviceService;
            this.SprintParticipantService = sprintParticipantService;
        }
        private ISprintParticipantService SprintParticipantService { get; }
        private ISprintService SprintService { get; }

        private IUserService UserService { get; }

        private IDeviceService DeviceService { get; }

        private IDashboardService DashboardService { get; }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <param name="searchTerm">Search term to filter</param>
        /// <param name="sortBy">Sort by to filter</param>
        /// <param name="filterBy">Term to filter</param>
        /// <returns>All public events available in database</returns>
        [HttpGet("get-public/{searchTerm}/{sortBy}/{filterBy}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<ResponseObject> GetAllPublicEvents(string searchTerm, string sortBy, string filterBy)
        {
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = await this.SprintService.GetAll((int)SprintType.PublicSprint, searchTerm, sortBy, filterBy),
            };
            return response;
        }

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>Toatal count of live events, 10-20KM and 21-30km</returns>
        [HttpGet("stat/live-events")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetLiveSprintCount()
        {
            LiveSprintCount liveSprintsCount = await this.SprintService.GetLiveSprintCount();
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = liveSprintsCount,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get all created sprints, query filter can be apply with form, to
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>All, Public, Private created sprint count for given date range </returns>
        [HttpGet("stat/created-events")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetCreatedEventsCount(DateTime from, DateTime? to)
        {
            CreatedSprintCount createdSprints = await this.SprintService.GetCreatedEventsCount(from, to);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = createdSprints,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="sprint">info about the sprint</param>
        /// <param name="repeatType">repeat type for the sprint</param>
        [HttpPost("create/{repeatType}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateSprintModel sprint, string repeatType)
        {
            User user = await this.User.GetUser(this.UserService);

            TimeSpan durationForTimeBasedEvent = new TimeSpan(0, 0, 0, 0);
            if (!string.IsNullOrEmpty(sprint.DurationForTimeBasedEvent))
            {
                durationForTimeBasedEvent = TimeSpan.Parse(sprint.DurationForTimeBasedEvent);
            }

            string encryptedEamil = null;
            if (sprint.InfluencerEmail != null)
            {
                var email = sprint.InfluencerEmail;
                encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);
            }
            int userId = await this.SprintService.GetInfluencerIdByEmail(encryptedEamil);

            if (userId != 0 || sprint.InfluencerEmail != null)
            {
                if (repeatType == "NONE")
                {
                    var result = await this.SprintService.CreateNewSprint(
                        user,
                        sprint.Name,
                        sprint.Distance,
                        sprint.StartTime,
                        sprint.SprintType,
                        sprint.NumberOfParticipants,
                        sprint.InfluencerEmail,
                        sprint.DraftEvent,
                        sprint.InfluencerAvailability,
                        sprint.ImageUrl,
                        sprint.promotionCode,
                        sprint.IsTimeBased,
                        durationForTimeBasedEvent
                        );
                    ResponseObject response = new ResponseObject()
                    {
                        StatusCode = (int)ApplicationResponseCode.Success,
                        Data = result,
                    };

                    if(response.StatusCode == (int)ApplicationResponseCode.Success)
                    {
                        var joinResult = await this.SprintParticipantService.JoinSprint(
                                result.SprintInfo.Id,
                                userId,
                                0,
                                true
                            );
                    }
                    return this.Ok(response);
                }
                else
                {
                    await this.SprintService.CreateMultipleSprints(
                        user,
                        sprint.Name,
                        sprint.Distance,
                        sprint.StartTime,
                        sprint.SprintType,
                        sprint.NumberOfParticipants,
                        sprint.InfluencerEmail,
                        sprint.DraftEvent,
                        sprint.InfluencerAvailability,
                        repeatType
                        );
                    ResponseObject response = new ResponseObject()
                    {
                        StatusCode = (int)ApplicationResponseCode.Success,
                        Data = null,
                    };
                    
                    return this.Ok(response);
                }
            }
            else
            {
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.NotExist,
                    Data = "User Not Exist",
                };
                return this.Ok(response);
            }
          
        }

        /// <summary>
        /// duplicate an event
        /// </summary>
        /// <param name="sprint">info about the sprint</param>
        [HttpPost("duplicate")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> DuplicateEvent([FromBody] CreateSprintModel sprint)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.DuplicateSprint(
              user,
              sprint.Name,
              sprint.Distance,
              sprint.StartTime,
              sprint.SprintType,
              sprint.NumberOfParticipants,
              sprint.InfluencerEmail,
              sprint.DraftEvent,
              sprint.InfluencerAvailability);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// drafts an event
        /// </summary>
        /// <param name="sprint">info about the sprint</param>
        /// <param name="repeatType">repeat type of the sprint</param>
        [HttpPost("draft/{repeatType}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> DraftEvent([FromBody] CreateSprintModel sprint, string repeatType)
        {
            User user = await this.User.GetUser(this.UserService);

            TimeSpan durationForTimeBasedEvent = new TimeSpan(0, 0, 0, 0);
            if (!string.IsNullOrEmpty(sprint.DurationForTimeBasedEvent))
            {
                durationForTimeBasedEvent = TimeSpan.Parse(sprint.DurationForTimeBasedEvent);
            }

            if (repeatType == "NONE")
            {
                var result = await this.SprintService.CreateNewSprint(
                    user,
                    sprint.Name,
                    sprint.Distance,
                    sprint.StartTime,
                    sprint.SprintType,
                    sprint.NumberOfParticipants,
                    sprint.InfluencerEmail,
                    sprint.DraftEvent,
                    sprint.InfluencerAvailability,
                    sprint.ImageUrl,
                    sprint.promotionCode,
                    sprint.IsTimeBased,
                    durationForTimeBasedEvent);
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result,
                };
                return this.Ok(response);
            }
            else
            {
                await this.SprintService.CreateMultipleSprints(
                    user,
                    sprint.Name,
                    sprint.Distance,
                    sprint.StartTime,
                    sprint.SprintType,
                    sprint.NumberOfParticipants,
                    sprint.InfluencerEmail,
                    sprint.DraftEvent,
                    sprint.InfluencerAvailability,
                    repeatType
                    );
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = null,
                };
                return this.Ok(response);
            }

            // User user = await this.User.GetUser(this.UserService);
            // var result = await this.SprintService.CreateNewSprint(
            //   user,
            //   sprint.Name,
            //   sprint.Distance,
            //   sprint.StartTime,
            //   sprint.SprintType,
            //   sprint.NumberOfParticipants,
            //   sprint.InfluencerEmail,
            //   sprint.DraftEvent,
            //   sprint.InfluencerAvailability);
            // ResponseObject response = new ResponseObject()
            // {
            //     StatusCode = (int)ApplicationResponseCode.Success,
            //     Data = result,
            // };
            // return this.Ok(response);
        }

        /// <summary>
        /// update sprint
        /// </summary>
        [HttpPut("update/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateSprintModel sprint, int sprintId)
        {
            User user = await this.User.GetUser(this.UserService);
            TimeSpan durationForTimeBasedEvent = new TimeSpan(0, 0, 0, 0);
            if (!string.IsNullOrEmpty(sprint.DurationForTimeBasedEvent))
            {
                durationForTimeBasedEvent = TimeSpan.Parse(sprint.DurationForTimeBasedEvent);
            }
            var result = await this.SprintService.UpdateSprint(
              user.Id,
              sprintId,
              sprint.Name,
              sprint.Distance,
              sprint.StartTime,
              sprint.NumberOfParticipants,
              sprint.InfluencerEmail,
              sprint.DraftEvent,
              sprint.ImageUrl,
              sprint.promotionCode,
              sprint.IsTimeBased,
              durationForTimeBasedEvent);

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };

            return this.Ok(response);
        }

        /// <summary>
        /// Get dashboard data
        /// </summary>
        /// <returns>Dashboard related data</returns>
        [HttpGet("stat/dashboard")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetDashboardData()
        {
            LiveSprintCount liveSprintsCount = await this.SprintService.GetLiveSprintCount();
            DeviceModal appdownloads = await this.DeviceService.GetDeviceInfo();
            DashboardDataDto dashboardData = this.DashboardService.GetDashboardData(liveSprintsCount, appdownloads);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = dashboardData,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Remove sprint with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to remove</param>
        [HttpPost("remove/{sprintId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> RemoveSprint(int sprintId)
        {
            User user = await this.User.GetUser(this.UserService);
            await this.SprintService.RemoveSprint(user.Id, sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Return report according to given time period
        /// </summary>
        /// <param name="timespan">timespan to generate the report</param>
        [AllowAnonymous]
        [HttpGet("getreport/{timespan}")]
        public async Task<IActionResult> GetReport(string timespan)
        {
            var reportData = await this.SprintService.GetReport(timespan);
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("sheetName");
                workSheet.Cells.LoadFromCollection(reportData, true);
                workSheet.Column(2).Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss AM/PM";
                workSheet.Cells["A1:G1"].Style.Font.Bold = true;
                //workSheet.Cells["A1:G1"].AutoFitColumns();
                package.Save();
            }

            stream.Position = 0;
            var contentType = "application/octet-stream";
            string fileName = $"ReportData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return this.File(stream, contentType, fileName);
        }

        // <summary>
        /// Get All Images related to public sprints
        /// </summary>
        /// <param name="timeOffset">time offset</param>
        /// <returns></returns>
        [HttpGet("GetAllImages")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<dynamic> GetAllImages()
        {
            var result = this.SprintService.GetAllImages();
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }


        /// <summary>
        /// Get All User Mails
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetAllUserMails")]
        public async Task<IActionResult> GetAllUserMails()
        {
            var reportData = await this.SprintService.GetAllUserMails();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("sheetName");
                workSheet.Cells.LoadFromCollection(reportData, true);
                workSheet.Column(2).Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss AM/PM";
                workSheet.Cells["A1:G1"].Style.Font.Bold = true;
                package.Encryption.Password = "sc275";
                package.Save();
            }

            stream.Position = 0;
            var contentType = "application/octet-stream";
            string fileName = $"EmailData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return this.File(stream, contentType, fileName);
        }
    }
}