namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Utils;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SocialShare;
    using SprintCrowd.BackEnd.Web.SocialShare;
    using SprintCrowdBackEnd.Enums;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Web.Event;
    using Serilog;
    using System.IO;
    using System.Net.Mail;
    using System.Net;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Domain.Sprint.Dtos;

    /// <summary>
    /// Sprint service
    /// </summary>
    public class SprintService : ISprintService
    {
        /// <summary>
        /// initializes an instance of SprintService
        /// </summary>
        /// <param name="sprintRepo">sprint repository</param>
        /// <param name="notificationClient">notification client</param>
        public SprintService(ISprintRepo sprintRepo, INotificationClient notificationClient, IUserRepo _userRepo, ISocialShareService socialShareService,
        ISprintParticipantRepo sprintParticipantRepo, ISprintParticipantService sprintParticipantService, IUserService userService)
        {
            this.SprintRepo = sprintRepo;
            this.NotificationClient = notificationClient;
            this.userRepo = _userRepo;
            this.SocialShareService = socialShareService;
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.SprintParticipantService = sprintParticipantService;
            this.UserService = userService;
        }
        private readonly IUserRepo userRepo;
        private ISprintParticipantService SprintParticipantService { get; }
        private ISocialShareService SocialShareService { get; }
        private ISprintRepo SprintRepo { get; }
        private INotificationClient NotificationClient { get; }
        private const int REPEAT_EVENTS_COUNT = 7;
        private ISprintParticipantRepo SprintParticipantRepo { get; }
        private IUserService UserService { get; set; }
        /// <summary>
        /// Get all events
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="searchTerm">Search term to filter</param>
        /// <param name="sortBy">Sort to filter</param>
        /// <param name="filterBy">Term to filter</param>
        /// <param name="pageNo">No of the page</param>
        /// <param name="limit">No of items per page</param>
        /// <returns>Available all events</returns>
        public async Task<SprintsPageDto> GetAll(int eventType, string searchTerm, string sortBy, string filterBy, int pageNo, int limit, int userId)
        {
            var result = await this.userRepo.GetUserRoleInfo(userId);
            return await this.SprintRepo.GetAllEvents(eventType, searchTerm, sortBy, filterBy, pageNo, limit, result , userId);


        }

        /// <summary>
        /// Get created sprint count for given date range
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>Created All, Public, Private sprints</returns>
        public async Task<CreatedSprintCount> GetCreatedEventsCount(DateTime? from, DateTime? to)
        {
            List<Sprint> allSprints = new List<Sprint>();
            if (from.HasValue && to.HasValue)
            {
                allSprints = await this.SprintRepo.GetAllEvents(from.Value, to.Value);
            }
            else
            {
                allSprints = await this.SprintRepo.GetAllEvents();
            }

            int totalCount = allSprints.Count();
            int privateCount = allSprints.Where(s => s.Type == (int)SprintType.PrivateSprint).Count();
            int publicCount = allSprints.Where(s => s.Type == (int)SprintType.PublicSprint).Count();
            return new CreatedSprintCount(totalCount, privateCount, publicCount);
        }

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>Toatal count of live events, 10-20KM and 21-30km</returns>
        public async Task<LiveSprintCount> GetLiveSprintCount()
        {
            List<Sprint> allSprints = await this.SprintRepo.GetLiveSprints();
            int all = allSprints.Count();
            int twoToTen = this.FilterWithDistance(allSprints, 2, 10).Count();
            int tenToTwenty = this.FilterWithDistance(allSprints, 11, 20).Count();
            int twentyOneToThirty = this.FilterWithDistance(allSprints, 21, 30).Count();
            int thirtyOneToFortyOne = this.FilterWithDistance(allSprints, 31, 41).Count();
            return new LiveSprintCount(all, twoToTen, tenToTwenty, twentyOneToThirty, thirtyOneToFortyOne);
        }

        /// <summary>
        /// Update Sprint Status By Sprint Id
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public bool UpdateSprintStatusBySprintId(int sprintId)
        {
            bool success = false;
            success = this.SprintRepo.UpdateSprintStatusBySprintId(sprintId) > 0 ? true : false;
            return success;
        }

        /// <summary>
        /// Join user to a sprint through an email
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> joinUser(int sprintId, string email)
        {
            int InfluncerUserId = 0;
            string encryptedEamil = null;
            if (email != null)
            {
                if (StringUtils.IsBase64String(email))
                {
                    encryptedEamil = email;
                    email = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(email);
                }
                else
                {
                    encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);
                }
            }
            InfluncerUserId = await this.GetInfluencerIdByEmail(encryptedEamil);
            if (InfluncerUserId == 0)
            {
                InfluncerUserId = await this.GetInfluencerIdByEmail(email);
            }
            try
            {
                await this.SprintParticipantService.JoinSprint(sprintId, InfluncerUserId, 0, true);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Participant adding errror - {e}");
                return false;
            }
        }


        /// <summary>
        /// Update instance of SprintService
        /// </summary>
        public async Task<UpdateSprintDto> UpdateSprint(
            int userId,
            int sprintId,
            CreateSprintModel sprintModel,
            TimeSpan durationForTimeBasedEvent,
            string descriptionForTimeBasedEvent)
        {



            if (sprintModel.promotionCode != null && sprintModel.promotionCode != string.Empty)
            {
                Sprint sprintPromoCode = await this.userRepo.IsPromoCodeExist(sprintModel.promotionCode);
                if (sprintPromoCode != null && sprintPromoCode.Id != sprintId)
                {
                    throw new SCApplicationException((int)SprintErrorCode.AlreadyExistPromoCode, "Already exist promotion Code");
                }
            }

            Expression<Func<Sprint, bool>> predicate = s => s.Id == sprintId;
            var sprintAavail = await this.SprintRepo.GetSprint(predicate);

            if (sprintAavail == null)
            {
                throw new Application.ApplicationException((int)SprintErrorCode.NotMatchingSprintWithId, "Sprint not found");
            }
            else if (sprintAavail.CreatedBy.Id != userId)
            {
                throw new Application.ApplicationException((int)SprintErrorCode.NotAllowedOperation, "Only creator can edit event");
            }
            else if (sprintAavail.StartDateTime.AddMinutes(-5) < DateTime.UtcNow)
            {
                throw new Application.ApplicationException((int)SprintErrorCode.CanNotEditSprint, "Can not edit this event");
            }

            if ((!String.Equals(sprintAavail.Name, sprintModel.Name) ||
                !string.Equals(sprintAavail.DescriptionForTimeBasedEvent, sprintModel.DescriptionForTimeBasedEvent) ||
                !String.Equals(sprintAavail.ImageUrl, sprintModel.ImageUrl) ||
                sprintAavail.IsSmartInvite != sprintModel.IsSmartInvite)
                && !sprintModel.IsSmartInvite)
            {
                var customData = new { campaign_name = "sprintshare", sprintId = sprintAavail.Id.ToString(), promotionCode = sprintAavail.PromotionCode };
                var socialLink = await this.SocialShareService.GetSmartLink(new SocialLink()
                {
                    Name = sprintModel.Name,
                    Description = descriptionForTimeBasedEvent,
                    ImageUrl = sprintModel.ImageUrl,
                    CustomData = customData
                });

                sprintAavail.SocialMediaLink = socialLink;
            }

            string oldName = sprintAavail.Name;
            if (sprintModel.Name != String.Empty)
            {
                sprintAavail.Name = sprintModel.Name;
            }
            if (sprintModel.Distance != null)
            {
                sprintAavail.Distance = (int)sprintModel.Distance;
            }
            if (sprintModel.StartTime != null)
            {
                sprintAavail.StartDateTime = (DateTime)sprintModel.StartTime;
            }
            if (sprintModel.Distance != null)
            {
                sprintAavail.Distance = (int)sprintModel.Distance;
            }
            if (sprintModel.NumberOfParticipants != null)
            {
                sprintAavail.NumberOfParticipants = (int)sprintModel.NumberOfParticipants;
            }

            sprintAavail.InfluencerAvailability = sprintModel.InfluencerAvailability;

            if (!string.IsNullOrEmpty(sprintModel.InfluencerEmail) && !string.Equals(sprintAavail.InfluencerEmail, sprintModel.InfluencerEmail)
            && !string.Equals(sprintAavail.InfluencerEmail, Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmail)))
            {
                sprintAavail.InfluencerEmail = !StringUtils.IsBase64String(sprintModel.InfluencerEmail) ?
                Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmail) : sprintModel.InfluencerEmail;
                sprintAavail.InfluencerAvailability = true;
                await this.joinUser(sprintId, sprintModel.InfluencerEmail);
            }

            if (!string.IsNullOrEmpty(sprintModel.InfluencerEmailSecond) && !string.Equals(sprintAavail.InfluencerEmailSecond, sprintModel.InfluencerEmailSecond))
            {
                sprintAavail.InfluencerEmailSecond = !StringUtils.IsBase64String(sprintModel.InfluencerEmailSecond) ?
                Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmailSecond) : sprintModel.InfluencerEmailSecond;
                await this.joinUser(sprintId, sprintModel.InfluencerEmailSecond);
            }

            if (sprintModel.DraftEvent != null)
            {
                sprintAavail.DraftEvent = (int)sprintModel.DraftEvent;

                if (sprintModel.DraftEvent == 0)
                {
                    sprintAavail.Status = (int)SprintStatus.NOTSTARTEDYET;
                }
                else
                {
                    sprintAavail.Status = (int)SprintStatus.NOTPUBLISHEDYET;
                }
            }
            sprintAavail.ImageUrl = sprintModel.ImageUrl;
            sprintAavail.VideoLink = sprintModel.VideoLink;
            sprintAavail.IsNarrationsOn = sprintModel.IsNarrationsOn;
            sprintAavail.VideoType = sprintModel.VideoType;
            // sprintAavail.PromotionCode = promotionCode;
            sprintAavail.IsTimeBased = sprintModel.IsTimeBased;
            sprintAavail.DurationForTimeBasedEvent = durationForTimeBasedEvent;
            sprintAavail.DescriptionForTimeBasedEvent = descriptionForTimeBasedEvent;
            


            var program = this.SprintRepo.GetSprintProgramDetailsByProgramId(sprintModel.ProgramId);
            //Add program Id only within the program start and end dates
            if (program.Result.StartDate <= sprintAavail.StartDateTime && sprintAavail.StartDateTime <= program.Result.StartDate.AddDays(program.Result.Duration * 7))
                sprintAavail.ProgramId = sprintModel.ProgramId;
            else
                sprintAavail.ProgramId = 0;


            if (sprintAavail.IsTimeBased == true)
            {
                sprintAavail.Interval = (int)sprintAavail.DurationForTimeBasedEvent.TotalMinutes;
                sprintAavail.Distance = 41000;
            }


            Sprint sprint = await this.SprintRepo.UpdateSprint(sprintAavail);
            this.SprintRepo.SaveChanges();


            this.NotificationClient.SprintNotificationJobs.SprintUpdate(
                sprint.Id,
                oldName,
                sprint.Name,
                sprint.Distance,
                sprint.StartDateTime,
                sprint.NumberOfParticipants,
                (SprintStatus)sprint.Status,
                (SprintType)sprint.Type,
                sprint.CreatedBy.Id
            );
            UpdateSprintDto result = new UpdateSprintDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                sprint.DraftEvent,
                sprint.InfluencerAvailability,
                sprint.InfluencerEmail,
                sprint.IsTimeBased,
                sprint.DurationForTimeBasedEvent,
                sprint.DescriptionForTimeBasedEvent);
            return result;
        }

        /// <summary>
        /// Validate Sprint Edit By SprintId
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public async Task<bool> ValidateSprintEditBySprintId(int sprintId)
        {
            Expression<Func<Sprint, bool>> predicate = s => s.Id == sprintId;
            var sprintAavail = await this.SprintRepo.GetSprint(predicate);
            if (sprintAavail != null)
            {
                if (sprintAavail.StartDateTime.AddMinutes(-5) < DateTime.UtcNow)
                    return false;
                else
                    return true;

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get Influencer User Id By Email
        /// </summary>
        /// <param name="infulenceEmail"></param>
        /// <returns></returns>
        public async Task<int> GetInfluencerIdByEmail(string infulenceEmail)
        {
            int infulenceId = 0;
            User userInfluencer = null;
            userInfluencer = await this.SprintRepo.FindInfluencer(infulenceEmail);
            if (userInfluencer != null)
            {
                infulenceId = userInfluencer.Id;
            }
            return infulenceId;
        }


        /// <summary>
        /// creates a new sprint
        /// </summary>

        public async Task<String> generatePromotionCode(bool isProgramCode = false)
        {
            int x = 0;
            string pCode = isProgramCode ? (await this.SprintRepo.GetLastSpecialSprintProgram()).ProgramCode : (await this.SprintRepo.GetLastSpecialSprint()).PromotionCode;
            if (Int32.TryParse(pCode, out x))
            {
                int promocode = Int32.Parse(pCode) + 1;
                string strPromocode = promocode.ToString("D6");
                return strPromocode;
            }
            string iniPromoCode = x.ToString("D6");
            return iniPromoCode;
        }

        /// <summary>
        /// Get Hosts Names
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        private async Task<string> GetHostsNames(Sprint sprint)
        {
           string hosts = string.Empty;
            User influencer = null;
            User influencerCoHost = null;
            if (sprint.Type == (int)SprintType.PublicSprint && sprint.InfluencerAvailability)
            {
                influencer = await this.SprintRepo.FindInfluencer(sprint.InfluencerEmail);
                if (influencer == null)
                    influencer = await this.SprintRepo.FindInfluencer(Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(sprint.InfluencerEmail));

                hosts = influencer.Name;

                if (sprint.InfluencerEmailSecond != null)
                {
                    influencerCoHost = await this.SprintRepo.FindInfluencer(sprint.InfluencerEmailSecond);
                    if (influencerCoHost == null)
                        influencerCoHost = await this.SprintRepo.FindInfluencer(Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(sprint.InfluencerEmailSecond));

                    hosts = hosts != string.Empty ? hosts + "," + influencerCoHost.Name : influencerCoHost.Name;
                }

            }
            return hosts;
        }
        /// <summary>
        /// Populate Email Body
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="url"></param>
        /// <param name="isEmail"></param>
        /// <returns></returns>
        private async Task<string> PopulateEmailBody(Sprint sprint , string repeatType )
        {
            string body = string.Empty;
            string leaderBoarLink = string.Empty;
            string editLink = string.Empty;
            //DO NOT REMOVE THIS-----Using following code generate the body HTML and assign to body
            //using (StreamReader reader = new StreamReader(Path.Combine("Domain\\Sprint", "EmailCrowd.html")))
            //{
            //    body = await reader.ReadToEndAsync();
            //}
            //Using above code generate the body HTML and assign to body

            body = "<!DOCTYPE html>\r\n<html>\r\n<table>\r\n<dev>\r\n<tbody>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse\">\r\n\r\n<tr>\r\n<td valign=\"top\" style=\"background:white;padding:0in 0in 0in 0in;background-size:cover;background-repeat:no-repeat;background:cover;background-size:cover\" id=\"m_-1894980969379046070templateHeader\">\r\n<div align=\"center\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;background-size:cover;background-repeat:no-repeat;background:#transparent none no-repeat center/cover;background-color:#transparent;background-size:cover\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 0in 0in 0in;max-width:600px!important\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 6.75pt 6.75pt 6.75pt;min-width:100%\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 6.75pt 0in 6.75pt\">\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center\"><img border=\"0\" width=\"287\" height=\"60\" style=\"width:2.9916in;height:.625in\" id=\"m_-1894980969379046070_x0000_i1028\" src=\"https://ci3.googleusercontent.com/proxy/jfOgoB-5mmJ6oQEXERu9HW8pIO8_1vAFJf3zFjJoZ4waMcRctlGNDqonrFK5Gn3iaqs9wancXSj5Kxq3XyUrqELkMkDhPHNJG4fLM3S9eY9cPErdG5TlrU8-t3Af9YE1Oj3klxKHikvN9PrnhPwOSgAtSZm4_w=s0-d-e1-ft#https://mcusercontent.com/51383202b4da2473e413e6f18/images/20fa3b70-4592-42b1-ac16-b96e3d5c0aa6.png\" class=\"CToWUd\"><u></u><u></u></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td valign=\"top\" style=\"background:white;padding:20.25pt 0in 40.5pt 0in;background-size:cover;background-repeat:no-repeat;background:cover;background-size:cover\" id=\"m_-1894980969379046070templateBody\">\r\n<div align=\"center\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:600px!important\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 0in 0in 0in;max-width:600px!important\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;background-size:cover;background-repeat:no-repeat;background:cover;background-size:cover\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 0in 0in 0in;min-width:100%\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:100%;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 13.5pt 6.75pt 13.5pt\">\r\n<p class=\"MsoNormal\" style=\"line-height:150%\"><strong><span style=\"font-size:18.0pt;line-height:150%;font-family:Roboto;color:black\">Event details for {SprintName}&nbsp;with Sprintcrowd</span></strong><span style=\"font-size:12.0pt;line-height:150%;font-family:&quot;Helvetica&quot;,sans-serif;color:#757575\"><u></u><u></u></span></p>\r\n<h3 style=\"margin:0in\">\r\n    <span style=\"font-size:12.0pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        <br>\r\n    </span><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        As a host you will be able to broadcast&nbsp;to all participants&nbsp;as well as a allowing others to talk by inviting them to the conversation while they run. View the LIVE statistics on the leaderboard&nbsp;to provide&nbsp;a more engaging&nbsp;experience&nbsp;for your runners or spectators.&nbsp;<br>\r\n        <br>\r\n        Below are&nbsp;the details of your event:\r\n    </span><span style=\"font-size:12.0pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        <br>\r\n        <br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Name</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : {SprintName}<br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Description</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : {Description}<br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Type</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : {SprintType}, {TimeorKm}<br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Hosts</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : {Hosts}<br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Accessbility</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : {Accessbility}<br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Start Time</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : {StartTime} &nbsp; &nbsp; &nbsp;: {WeeklyorOneTime}<br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Direct link to event for participants to join</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">\r\n        : <a class=\"MsoNormal\" href=\"{GetSocialLink}\"> {GetSocialLink}</a> <br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\">Link to web leaderboard for spectators</span></strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444;font-weight:normal\">: <a class=\"MsoNormal\" href=\"{LeaderBoarLink}\"> Click here</a> </span><span style=\"font-size:12.0pt;font-family:Roboto;color:#444444;font-weight:normal\"><u></u><u></u></span><br>\r\n    </span><strong><span style=\"font-size:10.5pt;font-family:Roboto;color:#444444\"><span style=\"font-size:12.0pt;font-family:Roboto;color:#444444;font-weight:normal\"><u></u><u></u></span>\r\n</h3>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<p class=\"MsoNormal\"><span style=\"display:none\"><u></u>&nbsp;<u></u></span></p>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 0in 0in 0in;word-break:break-word;word-break:break-word\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:100%;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 13.5pt 6.75pt 13.5pt\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<p class=\"MsoNormal\"><span style=\"display:none\"><u></u>&nbsp;<u></u></span></p>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 0in 0in 0in;word-break:break-word;word-break:break-word\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:100%;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 13.5pt 6.75pt 13.5pt\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<p class=\"MsoNormal\"><span style=\"display:none\"><u></u>&nbsp;<u></u></span></p>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 0in 0in 0in;word-break:break-word;word-break:break-word\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:100%;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 13.5pt 6.75pt 13.5pt\">\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center;line-height:150%\"><span style=\"font-size:10.0pt;line-height:150%;font-family:&quot;Helvetica&quot;,sans-serif;color:black\">For support contact us on:<br>\r\n<a href=\"mailto:hello@sprintcrowd.com\" target=\"_blank\">hello@sprintcrowd.com</a> or&nbsp;+46 (0) 8 599 05 900</span><span style=\"font-size:12.0pt;line-height:150%;font-family:&quot;Helvetica&quot;,sans-serif;color:#757575\"><u></u><u></u></span></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td valign=\"top\" style=\"background:#333333;padding:0in 0in 0in 0in;background-size:cover;background-repeat:no-repeat;background:cover;background-size:cover\" id=\"m_-1894980969379046070templateFooter\">\r\n<div align=\"center\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:600px!important\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 0in 0in 0in;max-width:600px!important\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;background-size:cover;background-repeat:no-repeat;background:#transparent none no-repeat center/cover;background-color:#transparent;background-size:cover\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 6.75pt 6.75pt 6.75pt;min-width:100%\">\r\n<div align=\"center\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"padding:0in 6.75pt 0in 6.75pt\">\r\n<div align=\"center\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 6.75pt 0in 6.75pt\">\r\n<div align=\"center\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 0in 0in 0in\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 7.5pt 6.75pt 0in\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td style=\"padding:3.75pt 7.5pt 3.75pt 6.75pt\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"0\" style=\"width:0in;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td width=\"24\" style=\"width:.25in;padding:0in 0in 0in 0in\">\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center\"><a href=\"https://www.facebook.com/virtualrunninglive/\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/virtualrunninglive/&amp;source=gmail&amp;ust=1628244582783000&amp;usg=AFQjCNGVn8JOHEz0KPf9nBhUxXRpSucR1Q\"><span style=\"text-decoration:none\"><img border=\"0\" width=\"24\" height=\"24\" style=\"width:.25in;height:.25in\" id=\"m_-1894980969379046070_x0000_i1027\" src=\"https://ci6.googleusercontent.com/proxy/iZE-48sXvszGHh6MUoqCYHnlP8ohfGJI6V1fj23YRaJHEaKjOb2V7stez03tl97kcCY9ebD52HlFfqGKcTQbPlQaysAL26ZKjUSa5NGX7CU3WUodCbzb-vFMkIXxvIREY4PT879oIw=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-facebook-48.png\" alt=\"Facebook\" class=\"CToWUd\"></span></a><u></u><u></u></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center\"><span style=\"display:none\"><u></u>&nbsp;<u></u></span></p>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 7.5pt 6.75pt 0in\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td style=\"padding:3.75pt 7.5pt 3.75pt 6.75pt\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"0\" style=\"width:0in;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td width=\"24\" style=\"width:.25in;padding:0in 0in 0in 0in\">\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center\"><a href=\"http://www.instagram.com/sprintcrowd\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=http://www.instagram.com/sprintcrowd&amp;source=gmail&amp;ust=1628244582783000&amp;usg=AFQjCNEk8gvXY_fm63KVwdmFHoi0CRL7UA\"><span style=\"text-decoration:none\"><img border=\"0\" width=\"24\" height=\"24\" style=\"width:.25in;height:.25in\" id=\"m_-1894980969379046070_x0000_i1026\" src=\"https://ci5.googleusercontent.com/proxy/Ihh9hEwk_36d3lzL_tLmGaqmGhc-dLqZP-II9LpKgUDCh37Kvw1N4-DJsrxuyAA9V1NNx3975BQO5w7DNVWvFHpPM4gkDm8eMVCLYy_PtGWEZAxMuaULgOR-6W0K_1sgXOcwNMtgGVE=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-instagram-48.png\" alt=\"Link\" class=\"CToWUd\"></span></a><u></u><u></u></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center\"><span style=\"display:none\"><u></u>&nbsp;<u></u></span></p>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 0in 6.75pt 0in\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td style=\"padding:3.75pt 7.5pt 3.75pt 6.75pt\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"0\" style=\"width:0in;border-collapse:collapse\">\r\n<tbody>\r\n<tr>\r\n<td width=\"24\" style=\"width:.25in;padding:0in 0in 0in 0in\">\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center\"><a href=\"http://www.sprintcrowd.com\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=http://www.sprintcrowd.com&amp;source=gmail&amp;ust=1628244582783000&amp;usg=AFQjCNH5aquG8N_19K-Cv0DJgVXr5E8WCg\"><span style=\"text-decoration:none\"><img border=\"0\" width=\"24\" height=\"24\" style=\"width:.25in;height:.25in\" id=\"m_-1894980969379046070_x0000_i1025\" src=\"https://ci6.googleusercontent.com/proxy/uZ0yuxmORppOSAVlAI9An9dTGgd5WLSQ0CBL7MLu_J4uk8Z1QO7RWFmdlkUYkmd_GLhwph5RoVCp9eKrXzEQnDQ91cNlGygasb_4p2fT-TnBvWoJAX8mqJXeyuG36Kto6QrY=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-link-48.png\" alt=\"Run connected\" class=\"CToWUd\"></span></a><u></u><u></u></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<p class=\"MsoNormal\"><span style=\"display:none\"><u></u>&nbsp;<u></u></span></p>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:6.75pt 0in 0in 0in;word-break:break-word;word-break:break-word\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;max-width:100%;min-width:100%\">\r\n<tbody>\r\n<tr>\r\n<td valign=\"top\" style=\"padding:0in 13.5pt 6.75pt 13.5pt\">\r\n<p class=\"MsoNormal\" align=\"center\" style=\"text-align:center;line-height:150%\"><span style=\"font-size:9.0pt;line-height:150%;font-family:&quot;Helvetica&quot;,sans-serif;color:white\"><br>\r\n<em><span style=\"font-family:&quot;Helvetica&quot;,sans-serif\">Copyright © 2020&nbsp;SPRINTCROWD AB. All rights reserved.</span></em><br>\r\n<br>\r\nWant to change how you receive these emails?<br>\r\nYou can <a href=\"https://sprintcrowd.us2.list-manage.com/profile?u=51383202b4da2473e413e6f18&amp;id=ccf8427935&amp;e=__test_email__&amp;c=f2d7791bdf\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://sprintcrowd.us2.list-manage.com/profile?u%3D51383202b4da2473e413e6f18%26id%3Dccf8427935%26e%3D__test_email__%26c%3Df2d7791bdf&amp;source=gmail&amp;ust=1628244582783000&amp;usg=AFQjCNElsfcKH9RbiP1FRIh2TMpRaAs2KA\">\r\n<span style=\"color:white\">update your preferences</span></a> or <a href=\"https://sprintcrowd.us2.list-manage.com/unsubscribe?u=51383202b4da2473e413e6f18&amp;id=ccf8427935&amp;e=__test_email__&amp;c=f2d7791bdf\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://sprintcrowd.us2.list-manage.com/unsubscribe?u%3D51383202b4da2473e413e6f18%26id%3Dccf8427935%26e%3D__test_email__%26c%3Df2d7791bdf&amp;source=gmail&amp;ust=1628244582783000&amp;usg=AFQjCNGTu0pdXr7an5Cu6mNhGVFZIc2aFQ\">\r\n<span style=\"color:white\">unsubscribe from this list</span></a>.<br>\r\n&nbsp; </span><span style=\"font-size:9.0pt;line-height:150%;font-family:&quot;Helvetica&quot;,sans-serif;color:white\"><u></u><u></u></span></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</html>";

            leaderBoarLink = "https://admin.sprintcrowd.com/control-panel/dashboard/leader_board?id=" + sprint.Id + "&name=" +sprint.Name+ "&stratTime="+ sprint.StartDateTime+ "&token=share";
           // editLink = "https://admin.sprintcrowd.com";

            body = body.Replace("{SprintName}", sprint.Name);
            body = body.Replace("{Description}", sprint.DescriptionForTimeBasedEvent);
            body = body.Replace("{SprintType}", sprint.IsTimeBased ? "Time Based" : "Distance Based");
            body = body.Replace("{TimeorKm}", sprint.IsTimeBased ? sprint.DurationForTimeBasedEvent.ToString() : (sprint.Distance/1000).ToString() +"km");
            body = body.Replace("{Hosts}", await this.GetHostsNames(sprint));
            body = body.Replace("{Accessbility}",  string.IsNullOrEmpty(sprint.PromotionCode) ? "PUBLIC (Anyone can join)" : "Invite only (Only people who have the link can join)");

            body = body.Replace("{StartTime}", TimeZoneInfo.ConvertTimeFromUtc(sprint.StartDateTime, TimeZoneInfo.Local).ToString("MMMM dd, yyyy hh:mm tt"));
            body = body.Replace("{WeeklyorOneTime}", string.IsNullOrEmpty(repeatType) ? string.Empty : ": " + repeatType);
            body = body.Replace("{GetSocialLink}", sprint.SocialMediaLink);
            body = body.Replace("{LeaderBoarLink}", leaderBoarLink);
            //body = body.Replace("{EditLink}", editLink);

            return body;
        }

        /// <summary>
        /// Send verification email
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> SendEmail(User user, Sprint sprint, string repeatType)
        {
            bool success = false;
            try
            {

                string userEmail = user.Email;
                if (StringUtils.IsBase64String(user.Email))
                {
                    userEmail = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(user.Email);
                }
                //userEmail = "gamithcsi@gmail.com";
                Console.WriteLine("start SendEmail");
                var fromAddress = new MailAddress("verify@sprintcrowd.com", "Sprintcrowd");
                var toAddress = new MailAddress(userEmail, user.Name);
                const string fromPassword = "x&7/3uUvXXzz#";

                string subject = await this.SprintParticipantRepo.GetUserSprintCount(user.Id) < 1 ? " Welcome to Sprintcrowd!" : "Sprintcrowd Team"; ;

                string body = await this.PopulateEmailBody(sprint, repeatType);
                var smtp = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    TargetName = "STARTTLS/smtp.office365.com"
                };
                smtp.Timeout = 30000;
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.Default,
                    IsBodyHtml = true
                })

                {
                    smtp.Send(message);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                throw ex;
            }
            return success;
        }

        public async Task<CreateSprintDto> CreateNewSprint(
            User user,
            CreateSprintModel sprintModel,
            TimeSpan durationForTimeBasedEvent,
            string descriptionForTimeBasedEvent,string repeatType ,bool isCrowdRun = false)
        {

            Sprint sprint = new Sprint();

            if (!string.IsNullOrEmpty(sprintModel.InfluencerEmail))
            {
                if (!StringUtils.IsBase64String(sprintModel.InfluencerEmail))
                {
                    sprint.InfluencerEmail = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmail);
                }
                else
                {
                    sprint.InfluencerEmail = sprintModel.InfluencerEmail;
                }
            }

            if (!string.IsNullOrEmpty(sprintModel.InfluencerEmailSecond))
            {
                if (!StringUtils.IsBase64String(sprintModel.InfluencerEmailSecond))
                {
                    sprint.InfluencerEmailSecond = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmailSecond);
                }
                else
                {
                    sprint.InfluencerEmailSecond = sprintModel.InfluencerEmailSecond;
                }

            }

            if (sprintModel.promotionCode != null && sprintModel.promotionCode != string.Empty)
            {
                Sprint sprintPromoCode = await this.userRepo.IsPromoCodeExist(sprintModel.promotionCode);
                if (sprintPromoCode != null)
                {
                    throw new SCApplicationException((int)SprintErrorCode.AlreadyExistPromoCode, "Already exist promotion Code");
                }
            }

            // if (type == (int)SprintType.PrivateSprint)
            // {
            //     Expression<Func<Sprint, bool>> predicate = s => s.CreatedBy.Id == user.Id && s.StartDateTime > DateTime.UtcNow && s.Status != (int)SprintStatus.ARCHIVED;
            //     var isAlreadyCreatedSprint = await this.SprintRepo.GetSprint(predicate);
            //     if (isAlreadyCreatedSprint != null)
            //     {
            //         throw new SCApplicationException((int)SprintErrorCode.AlreadyExistSprint, "Already exist event");
            //     }
            // }

            sprint.SocialMediaLink = string.Empty;
            sprint.Name = sprintModel.Name;

            if (sprintModel.IsTimeBased)
            {
                sprint.Distance = 41000;
            }
            else
            {
                sprint.Distance = sprintModel.Distance;
            }

            sprint.StartDateTime = sprintModel.StartTime;
            sprint.CreatedBy = user;
            sprint.IsNarrationsOn = sprintModel.IsNarrationsOn;
            sprint.Type = sprintModel.SprintType;
            sprint.NumberOfParticipants = sprintModel.NumberOfParticipants == null ? NumberOfParticipants(sprintModel.SprintType) : (int)sprintModel.NumberOfParticipants;
            sprint.InfluencerAvailability = sprintModel.InfluencerAvailability;
            sprint.IsNarrationsOn = sprintModel.IsNarrationsOn;
            sprint.DraftEvent = sprintModel.DraftEvent;
            sprint.ImageUrl = sprintModel.ImageUrl;
            sprint.PromotionCode = sprintModel.promotionCode == "PROMO" ? await this.generatePromotionCode() : null;
            sprint.IsSmartInvite = sprintModel.IsSmartInvite;
            sprint.IsTimeBased = sprintModel.IsTimeBased;
            sprint.DurationForTimeBasedEvent = durationForTimeBasedEvent;
            sprint.DescriptionForTimeBasedEvent = descriptionForTimeBasedEvent;
            sprint.VideoLink = sprintModel.VideoLink;
            sprint.VideoType = sprintModel.VideoType;
            sprint.IsSoloRun = sprintModel.IsSoloRun;
            sprint.ProgramId = sprintModel.ProgramId;

            if (sprint.IsTimeBased == true)
            {
                sprint.Interval = (int)sprint.DurationForTimeBasedEvent.TotalMinutes;
            }


            if (sprintModel.DraftEvent == 0)
            {
                sprint.Status = (int)SprintStatus.NOTSTARTEDYET;

            }
            else
            {
                sprint.Status = (int)SprintStatus.NOTPUBLISHEDYET;
            }

            Sprint addedSprint = await this.SprintRepo.AddSprint(sprint);
            if (sprintModel.SprintType == (int)SprintType.PrivateSprint)
            {
                await this.SprintRepo.AddParticipant(user.Id, addedSprint.Id, ParticipantStage.JOINED);
            }

            this.SprintRepo.SaveChanges();

            this.NotificationClient.NotificationReminderJobs.TimeReminder(
                addedSprint.Id,
                addedSprint.Name,
                addedSprint.Distance,
                addedSprint.StartDateTime,
                addedSprint.NumberOfParticipants,
                (SprintType)addedSprint.Type,
                (SprintStatus)addedSprint.Status);

            if (sprintModel.DraftEvent == 0)
            {
                     var customData = new
                     {
                         campaign_name = "sprintshare",
                         sprintId = sprint.Id.ToString(),
                         promotionCode = sprint.PromotionCode,
                         name = addedSprint.Name,
                         distance = addedSprint.Distance > 0 ? (addedSprint.Distance/1000).ToString() : addedSprint.Distance.ToString(),
                         startDateTime = addedSprint.StartDateTime.ToString(),
                         type = addedSprint.Type.ToString(),
                         extendedTime = addedSprint.StartDateTime.AddMinutes(addedSprint.Interval).ToString(),
                         descriptionForTimeBasedEvent = addedSprint.DescriptionForTimeBasedEvent
                     };
                try
                {
                    var socialLink = sprintModel.IsSmartInvite ?
                    await this.SocialShareService.updateTokenAndGetInvite(customData) :
                    await this.SocialShareService.GetSmartLink(new SocialLink()
                    {
                        Name = sprintModel.Name,
                        Description = descriptionForTimeBasedEvent,
                        ImageUrl = sprintModel.ImageUrl,
                        CustomData = customData
                    });

                    sprint.SocialMediaLink = socialLink;
                    if (!string.IsNullOrEmpty(sprintModel.InfluencerEmail))
                    {
                        await this.joinUser(sprint.Id, sprint.InfluencerEmail);
                    }
                    if (!string.IsNullOrEmpty(sprintModel.InfluencerEmailSecond))
                    {
                        await this.joinUser(sprint.Id, sprint.InfluencerEmailSecond);
                    }
                    if(isCrowdRun && repeatType == "NONE")
                    await this.SendEmail(user, sprint, string.Empty);
                    await this.SprintRepo.UpdateSprint(sprint);


                }

                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return CreateSprintDtoMapper(sprint, user);
        }

        /// <summary>
        /// Create multiple sprints
        /// </summary>
        public async Task CreateMultipleSprints(
            User user,
            CreateSprintModel sprint,
            TimeSpan durationForTimeBasedEvent,
            String repeatType, bool isCrowdRun = false)
        {
            Sprint sprintInfo = new Sprint();
            int incementalSprintNumber = 0;
            string sprintName = sprint.Name;
            var program = this.SprintRepo.GetSprintProgramDetailsByProgramId(sprint.ProgramId);
            if (repeatType == "DAILY")
            {
                DateTime endDate = sprint.StartTime.AddDays(REPEAT_EVENTS_COUNT);
                while (sprint.StartTime < endDate)
                {
                    incementalSprintNumber++;
                    sprint.Name = sprintName + " (" + incementalSprintNumber + ")";
                    await this.CreateNewSprint(user, sprint, durationForTimeBasedEvent, sprint.DescriptionForTimeBasedEvent, repeatType, isCrowdRun);
                    sprint.StartTime = sprint.StartTime.AddDays(1);

                    //Add program Id only within the program start and end dates
                    if(program.Result.StartDate <= sprint.StartTime && sprint.StartTime <= program.Result.StartDate.AddDays(program.Result.Duration * 7))
                    sprint.ProgramId = sprint.ProgramId;
                }
            }
            else if (repeatType == "WEEKLY")
            {
                DateTime endDate = sprint.StartTime.AddDays(7 * REPEAT_EVENTS_COUNT);
                while (sprint.StartTime < endDate)
                {
                    incementalSprintNumber++;
                    sprint.Name = sprintName + " (" + incementalSprintNumber + ")";
                    await this.CreateNewSprint(user, sprint, durationForTimeBasedEvent, sprint.DescriptionForTimeBasedEvent, repeatType, isCrowdRun);
                    sprint.StartTime = sprint.StartTime.AddDays(7);

                    //Add program Id only within the program start and end dates
                    if (program.Result.StartDate <= sprint.StartTime && sprint.StartTime <= program.Result.StartDate.AddDays(program.Result.Duration * 7))
                        sprint.ProgramId = sprint.ProgramId;
                }
            }
            else if (repeatType == "MONTHLY")
            {
                DateTime endDate = sprint.StartTime.AddMonths(REPEAT_EVENTS_COUNT);
                while (sprint.StartTime < endDate)
                {
                    incementalSprintNumber++;
                    sprint.Name = sprintName + " (" + incementalSprintNumber + ")";
                    await this.CreateNewSprint(user, sprint, durationForTimeBasedEvent, sprint.DescriptionForTimeBasedEvent, repeatType, isCrowdRun);
                    sprint.StartTime = sprint.StartTime.AddMonths(1);

                    //Add program Id only within the program start and end dates
                    if (program.Result.StartDate <= sprint.StartTime && sprint.StartTime <= program.Result.StartDate.AddDays(program.Result.Duration * 7))
                        sprint.ProgramId = sprint.ProgramId;
                }
            }

            sprintInfo.Name = sprint.Name;
            sprintInfo.IsTimeBased = sprint.IsTimeBased;
            sprintInfo.DescriptionForTimeBasedEvent = sprint.DescriptionForTimeBasedEvent;
            sprintInfo.DurationForTimeBasedEvent = TimeSpan.Parse(sprint.DurationForTimeBasedEvent);
            sprintInfo.Distance = sprint.Distance;
            sprintInfo.PromotionCode = sprint.promotionCode;
            sprintInfo.StartDateTime = sprint.StartTime;
            sprintInfo.SocialMediaLink = sprint.SocialMediaLink;

            

            if (isCrowdRun)
            await this.SendEmail(user, sprintInfo, repeatType);
        }

        /// <summary>
        /// Duplicate a sprint
        /// </summary>
        public async Task<CreateSprintDto> DuplicateSprint(
            User user,
            string name,
            int distance, DateTime startTime,
            int type,
            int? numberOfParticipants,
            string infulenceEmail,
            int draft,
            bool influencerAvailability)
        {

            if (infulenceEmail != null)
            {
                var email = infulenceEmail;
                var encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);

                infulenceEmail = encryptedEamil;
            }

            Sprint duplicatedSprint = new Sprint();
            List<string> existingSprintNames = await this.SprintRepo.GetSprintNames(name);

            if (existingSprintNames.Any())
            {
                //string[] splitSprintnames = existingSprintNames.Last().Split("_");  //sprintName.Split("_");
                //var lastElement = splitSprintnames[splitSprintnames.Length - 1];

                //recursive duplicate related code goes here
                string lastElement = existingSprintNames.Last().Split(new char[] { '(', ')' })[1];

                if (int.TryParse(lastElement, out int incementalNumber))
                {
                    incementalNumber++;
                    string newSprintName = name + "(" + incementalNumber.ToString() + ")";

                    duplicatedSprint.Name = newSprintName;
                    duplicatedSprint.Distance = distance;
                    duplicatedSprint.StartDateTime = startTime;
                    duplicatedSprint.CreatedBy = user;
                    duplicatedSprint.Type = type;
                    duplicatedSprint.Status =
                        draft == 0 ? (int)SprintStatus.NOTSTARTEDYET : (int)SprintStatus.NOTPUBLISHEDYET;
                    duplicatedSprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                    duplicatedSprint.InfluencerAvailability = influencerAvailability;
                    duplicatedSprint.InfluencerEmail = infulenceEmail;
                    duplicatedSprint.DraftEvent = draft;
                    if (duplicatedSprint.IsTimeBased == true)
                    {
                        duplicatedSprint.Interval = (int)duplicatedSprint.DurationForTimeBasedEvent.TotalMinutes;
                        duplicatedSprint.Distance = 41000;
                    }

                    Sprint addedSprint = await this.SprintRepo.AddSprint(duplicatedSprint);
                }
            }
            else
            {
                //first time duplicate code goes here
                string incementalNumber = "1";
                string newSprintName = name + "(" + incementalNumber + ")";

                duplicatedSprint.Name = newSprintName;
                duplicatedSprint.Distance = distance;
                duplicatedSprint.StartDateTime = startTime;
                duplicatedSprint.CreatedBy = user;
                duplicatedSprint.Type = type;
                duplicatedSprint.Status =
                    draft == 0 ? (int)SprintStatus.NOTSTARTEDYET : (int)SprintStatus.NOTPUBLISHEDYET;
                duplicatedSprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                duplicatedSprint.InfluencerAvailability = influencerAvailability;
                duplicatedSprint.InfluencerEmail = infulenceEmail;
                duplicatedSprint.DraftEvent = draft;

                if (duplicatedSprint.IsTimeBased == true)
                {
                    duplicatedSprint.Interval = (int)duplicatedSprint.DurationForTimeBasedEvent.TotalMinutes;
                }

                Sprint addedSprint = await this.SprintRepo.AddSprint(duplicatedSprint);

            }
            this.SprintRepo.SaveChanges();
            return CreateSprintDtoMapper(duplicatedSprint, user);
        }

        /// <summary>
        /// Get sprint with pariticipants by creator id
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="extendedTime"> extended time </param>
        /// <returns><see cref="SprintWithPariticpantsDto"> sprint details with paritipants</see></returns>
        public async Task<SprintWithPariticpantsDto> GetSprintByCreator(int userId, int? extendedTime)
        {
            var now = extendedTime == null ? DateTime.UtcNow : DateTime.UtcNow.AddMinutes((int)extendedTime);
            Expression<Func<Sprint, bool>> predicate = s => s.CreatedBy.Id == userId && s.Status != (int)SprintStatus.ARCHIVED && s.StartDateTime > now;
            var sprint = await this.SprintRepo.GetSprint(predicate);
            if (sprint == null)
            {
                return null;
            }
            else
            {
                Expression<Func<SprintParticipant, bool>> participantPredicate = s => s.SprintId == sprint.Id && s.User.UserState == UserState.Active;
                var participants = this.SprintRepo.GetParticipants(participantPredicate);
                return SprintWithPariticpantsMapper(sprint, participants.ToList());
            }
        }

        /// <summary>
        /// Get the sprint details and sprint participant details with given
        /// sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="pageNo">pageNo for pagination</param>
        /// <param name="limit">limit for the page for pagination</param>
        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        public async Task<SprintWithPariticpantsDto> GetSprintWithPaticipants(int sprintId, int pageNo, int limit)
        {
            Expression<Func<Sprint, bool>> sprintPredicate = s => s.Id == sprintId;
            var sprint = await this.SprintRepo.GetSprint(sprintPredicate);
            var expireDate = DateTime.UtcNow.AddHours(-8);
            Expression<Func<SprintParticipant, bool>> participantPredicate = s =>
                s.SprintId == sprintId &&
            s.User.UserState == UserState.Active &&
            s.Sprint.StartDateTime > expireDate &&
            s.User.Name != string.Empty &&
            (s.Stage != ParticipantStage.QUIT && s.Stage != ParticipantStage.DECLINE);

            List<SprintParticipant> pariticipants = null;

            if (pageNo == 0 && limit == 0)
            {
                pariticipants = this.SprintRepo.GetParticipants(participantPredicate).ToList();
            }
            else
            {
                pariticipants = this.SprintRepo.GetParticipants(participantPredicate).Skip(pageNo * limit).Take(limit).ToList();
            }

            User influencer = null;
            User influencerCoHost = null;
            if (sprint.Type == (int)SprintType.PublicSprint && sprint.InfluencerAvailability)
            {
                influencer = await this.SprintRepo.FindInfluencer(sprint.InfluencerEmail);
                if (influencer == null)
                    influencer = await this.SprintRepo.FindInfluencer(Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(sprint.InfluencerEmail));

                if (sprint.InfluencerEmailSecond != null)
                {
                    influencerCoHost = await this.SprintRepo.FindInfluencer(sprint.InfluencerEmailSecond);
                    if (influencerCoHost == null)
                        influencerCoHost = await this.SprintRepo.FindInfluencer(Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(sprint.InfluencerEmailSecond));
                }
            }
            return SprintWithPariticpantsMapper(sprint, pariticipants.ToList(), influencer, influencerCoHost);
        }

        /// <summary>
        /// Get Sprint Paticipants
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public async Task<List<SprintParticipant>> GetSprintPaticipants(int sprintId, int pageNo, int limit, bool? completed)
        {
            Expression<Func<SprintParticipant, bool>> participantPredicate = null;

            if (completed == null)
            {
                participantPredicate = s =>
                s.SprintId == sprintId && s.User.Name != string.Empty;
            }
            else
            {
                if ((bool)completed)
                {
                    participantPredicate = s =>
                      s.SprintId == sprintId && s.User.Name != string.Empty && s.Stage == ParticipantStage.COMPLETED;
                }
                else
                {
                    participantPredicate = s =>
                      s.SprintId == sprintId && s.User.Name != string.Empty && s.Stage != ParticipantStage.COMPLETED;
                }
            }



           var participantList = this.SprintRepo.GetParticipants(participantPredicate).Skip(pageNo * limit).Take(limit).ToList();

            foreach (SprintParticipant participant in participantList)
            {
                if (!string.IsNullOrEmpty(participant.RaceCompletedDuration))
                {
                    participant.RaceCompletedDuration = TimeSpan.Parse(participant.RaceCompletedDuration).ToString(@"hh\:mm\:ss");
                }
            }

            return participantList.OrderBy(participants => participants.Position).ToList();

        }


        /// <summary>
        /// Get Sprint Paticipants Counts
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public async Task<int> GetSprintPaticipantsCounts(int sprintId)
        {
            Expression<Func<SprintParticipant, bool>> participantPredicate = s =>
               s.SprintId == sprintId && s.User.Name != string.Empty;

            return this.SprintRepo.GetParticipants(participantPredicate).ToList().Count;
        }

        /// <summary>
        /// Remove sprint
        /// </summary>
        /// <param name="userId">creator id </param>
        /// <param name="sprintId">sprint id to remove</param>
        public async Task Remove(int userId, int sprintId)
        {
            Expression<Func<Sprint, bool>> sprintPredicate = s => s.Id == sprintId;
            var sprint = await this.SprintRepo.GetSprint(sprintPredicate);
            if (sprint == null)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotMatchingSprintWithId, "Sprint not found with given id");
            }
            else if (sprint.CreatedBy.Id != userId)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotAllowedOperation, "Only creator can delete event");
            }
            else if (sprint.StartDateTime.AddMinutes(-5) < DateTime.UtcNow)
            {
                throw new SCApplicationException((int)SprintErrorCode.MarkAttendanceEnable, "Mark attendance enable");
            }
            else
            {
                sprint.Status = (int)SprintStatus.ARCHIVED;
                await this.SprintRepo.UpdateSprint(sprint);
                this.SprintRepo.SaveChanges();
                this.NotificationClient.SprintNotificationJobs.SprintRemove(
                    sprint.Id,
                    sprint.Name,
                    sprint.Distance,
                    sprint.StartDateTime,
                    sprint.NumberOfParticipants,
                    (SprintStatus)sprint.Status,
                    (SprintType)sprint.Type,
                    sprint.CreatedBy.Id,
                    sprint.CreatedBy.Name,
                    sprint.CreatedBy.ProfilePicture,
                    sprint.CreatedBy.Code,
                    sprint.CreatedBy.ColorCode,
                    sprint.CreatedBy.City,
                    sprint.CreatedBy.Country,
                    sprint.CreatedBy.CountryCode);
            }
        }

        /// <summary>
        /// Remove sprint from Admin Panel
        /// </summary>
        /// <param name="userId">creator id </param>
        /// <param name="sprintId">sprint id to remove</param>
        public async Task RemoveSprint(int userId, int sprintId)
        {
            Expression<Func<Sprint, bool>> sprintPredicate = s => s.Id == sprintId;
            var sprint = await this.SprintRepo.GetSprint(sprintPredicate);
            if (sprint == null)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotMatchingSprintWithId, "Sprint not found with given id");
            }
            else if (sprint.CreatedBy.Id != userId)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotAllowedOperation, "Only creator can delete event");
            }
            else if (sprint.ProgramId > 0)
            {
                 this.SprintRepo.RemoveSprint(sprint);
            }
            else
            {
                sprint.Status = (int)SprintStatus.ARCHIVED;
                await this.SprintRepo.UpdateSprint(sprint);
                this.SprintRepo.SaveChanges();
                this.NotificationClient.SprintNotificationJobs.SprintRemove(
                    sprint.Id,
                    sprint.Name,
                    sprint.Distance,
                    sprint.StartDateTime,
                    sprint.NumberOfParticipants,
                    (SprintStatus)sprint.Status,
                    (SprintType)sprint.Type,
                    sprint.CreatedBy.Id,
                    sprint.CreatedBy.Name,
                    sprint.CreatedBy.ProfilePicture,
                    sprint.CreatedBy.Code,
                    sprint.CreatedBy.ColorCode,
                    sprint.CreatedBy.City,
                    sprint.CreatedBy.Country,
                    sprint.CreatedBy.CountryCode);
            }
        }

        public async Task InviteRequest(int inviterId, int inviteeId, int sprintId)
        {
            await this.SprintRepo.AddParticipant(inviteeId, sprintId);
        }

        private List<Sprint> FilterWithDistance(List<Sprint> sprints, int from, int to)
        {
            return sprints
                .Where(s => s.Distance >= from * 1000 && s.Distance <= to * 1000).ToList();
        }

        private static int NumberOfParticipants(int sprintType)
        {
            if (sprintType == (int)SprintType.PrivateSprint)
            {
                return 10;
            }
            if (sprintType == (int)SprintType.PublicSprint)
            {
                return 100;
            }
            throw new Application.ApplicationException("Invalid sprint type");
        }

        public static CreateSprintDto CreateSprintDtoMapper(Sprint sprint, User user)
        {
            CreateSprintDto result = new CreateSprintDto(
                sprint,
                user,
                true,
                ParticipantStage.JOINED);
            return result;
        }

        public static SprintWithPariticpantsDto SprintWithPariticpantsMapper(Sprint sprint, List<SprintParticipant> participants, User influencer = null, User influencerCoHost = null)
        {
            string strCoHost = string.Empty;
            if (sprint.InfluencerEmailSecond != null && sprint.InfluencerEmailSecond.Trim() != string.Empty && StringUtils.IsBase64String(sprint.InfluencerEmailSecond))
                strCoHost = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(sprint.InfluencerEmailSecond);

            SprintWithPariticpantsDto result = new SprintWithPariticpantsDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                sprint.Location,
                sprint.PromotionCode,
                sprint.IsTimeBased,
                sprint.DurationForTimeBasedEvent,
                sprint.DescriptionForTimeBasedEvent,
                sprint.InfluencerAvailability,
                sprint.IsNarrationsOn,
                strCoHost,
                sprint.Interval);

            participants
                .ForEach(p =>
                {
                    var isInfulencer = false;
                    if (influencer != null)
                    {
                        isInfulencer = influencer.Id == p.UserId;
                    }
                    if (influencerCoHost != null && !isInfulencer)
                    {
                        isInfulencer = influencerCoHost.Id == p.UserId;
                    }
                    result.AddParticipant(
                        p.User.Id,
                        p.User.Name,
                        p.User.ProfilePicture,
                        p.User.City,
                        p.User.Country,
                        p.User.CountryCode,
                        p.User.ColorCode,
                        p.User.Id == sprint.CreatedBy.Id,
                        p.Stage,
                        isInfulencer
                    );
                });
            return result;
        }

        /// <summary>
        /// Get public sprint with user preference
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="timeOffset">time offset to utc</param>
        /// <returns>sprint with participant info</returns>
        public async Task<List<PublicSprintWithParticipantsDto>> GetPublicSprints(int userId, int timeOffset)
        {
            var userPreference = await this.SprintRepo.GetUserPreference(userId);
            var query = new PublicSprintQueryBuilder(userPreference).Build(timeOffset);
            IEnumerable<Sprint> sprints = await this.SprintRepo.GetSprints(query);
            var sprintDto = new List<PublicSprintWithParticipantsDto>();
            var friendsRelations = this.SprintRepo.GetFriends(userId);
            var friends = friendsRelations.Select(f => f.AcceptedUserId == userId ? f.SharedUserId : f.AcceptedUserId);
            foreach (var sprint in sprints)
            {

                var participants = sprint.Participants.Where(s => s.User.UserState == UserState.Active &&
                    (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE || s.Stage == ParticipantStage.COMPLETED));

                if (!participants.Any(p => p.UserId == userId))
                {
                    var resultDto = new PublicSprintWithParticipantsDto(sprint.Id, sprint.Name, sprint.Distance, sprint.NumberOfParticipants, sprint.StartDateTime, (SprintType)sprint.Type, sprint.Location, sprint.ImageUrl, sprint.PromotionCode);
                    foreach (var participant in participants)
                    {
                        resultDto.AddParticipant(
                            participant.User.Id,
                            participant.User.Name,
                            participant.User.ProfilePicture,
                            participant.User.City,
                            participant.User.Country,
                            participant.User.CountryCode,
                            participant.User.ColorCode,
                            false,
                            participant.Stage,
                            friends.Contains(participant.UserId));

                    }
                    sprintDto.Add(resultDto);
                }
            }
            return sprintDto;
        }


        private ParticipantStage GetStage(int? status)
        {
            ParticipantStage stage = ParticipantStage.JOINED;
            if (status == null)
            {
                stage = ParticipantStage.JOINED;
            }
            if (status == 0)
            {
                stage = ParticipantStage.JOINED;
            }
            if (status == 1)
            {
                stage = ParticipantStage.PENDING;
            }
            return stage;
        }

        public async Task<List<PublicSprintWithParticipantsDto>> GetOpenEvents(int? status, int userId, int timeOffset, int pageNo, int limit)
        {

            try
            {
                var userPreference = await this.SprintRepo.GetUserPreference(userId);
                var query = new PublicSprintQueryBuilder(userPreference).BuildOpenEvents(timeOffset);

                IEnumerable<Sprint> openEvents = null;
                //get events not assign to program (if assign to program programId > 0)
                openEvents = this.SprintRepo.GetSprint_Open(query).OrderBy(x => x.StartDateTime).Where(s => s.ProgramId == 0);

                var sprintDto = new List<PublicSprintWithParticipantsDto>();
                var friendsRelations = this.SprintRepo.GetFriends(userId);
                var friends = friendsRelations.Select(f => f.AcceptedUserId == userId ? f.SharedUserId : f.AcceptedUserId);

                foreach (var sprint in openEvents)
                {

                    if ((int)status == (int)OpenEventJoinStatus.NOTJOINED)
                    {


                        if (sprint.Participants.Where(s =>
                                      s.User.UserState == UserState.Active &&
                                      s.Stage != ParticipantStage.DECLINE && s.Stage != ParticipantStage.QUIT && s.UserId == userId).Any())
                        {
                            continue;
                        }
                    }

                    if (sprint.PromotionCode == null || sprint.PromotionCode == string.Empty)
                    {
                        List<SprintParticipant> participants = null;

                        if (!status.HasValue)
                        {
                            participants = sprint.Participants.Where(s =>
                           s.User.UserState == UserState.Active &&
                           s.Stage != ParticipantStage.DECLINE && s.Stage != ParticipantStage.QUIT).ToList();
                        }
                        else
                        {
                            if ((int)status == (int)OpenEventJoinStatus.All)
                            {
                                participants = sprint.Participants.Where(s =>
                                s.User.UserState == UserState.Active &&
                                s.Stage != ParticipantStage.DECLINE && s.Stage != ParticipantStage.QUIT).ToList();
                            }
                            else
                            {
                                if ((int)status == (int)OpenEventJoinStatus.JOINED)
                                {
                                    participants = sprint.Participants.Where(s =>
                                    s.User.UserState == UserState.Active &&
                                    s.Stage != ParticipantStage.DECLINE && s.Stage != ParticipantStage.QUIT && s.UserId == userId).ToList();

                                    if (participants.Count > 0)
                                    {
                                        var resultDto = new PublicSprintWithParticipantsDto(
                                   sprint.Id, sprint.Name, sprint.Distance,
                                   sprint.NumberOfParticipants, sprint.StartDateTime,
                                   (SprintType)sprint.Type, sprint.Location, sprint.ImageUrl, sprint.PromotionCode, sprint.IsTimeBased, sprint.DurationForTimeBasedEvent, sprint.DescriptionForTimeBasedEvent, sprint.Interval);
                                        foreach (var participant in participants)
                                        {
                                            resultDto.AddParticipant(
                                                participant.User.Id,
                                                participant.User.Name,
                                                participant.User.ProfilePicture,
                                                participant.User.City,
                                                participant.User.Country,
                                                participant.User.CountryCode,
                                                participant.User.ColorCode,
                                                false,
                                                ParticipantStage.JOINED,
                                                friends.Contains(participant.User.Id));

                                        }
                                        resultDto.SprintInfo.IsProgramSprint = sprint.ProgramId > 0 ? true : false;
                                        resultDto.SprintInfo.ProgramId = sprint.ProgramId;
                                        sprintDto.Add(resultDto);
                                    }
                                }
                                if ((int)status == (int)OpenEventJoinStatus.NOTJOINED)
                                {
                                    participants = sprint.Participants.Where(s =>
                                    s.User.UserState == UserState.Active &&
                                    s.Stage != ParticipantStage.DECLINE && s.Stage != ParticipantStage.QUIT && s.UserId != userId).ToList();
                                }
                            }
                        }



                        if (!participants.Any(p => p.Id == userId) && status != 0)
                        {

                            var resultDto = new PublicSprintWithParticipantsDto(
                                sprint.Id, sprint.Name, sprint.Distance,
                                sprint.NumberOfParticipants, sprint.StartDateTime,
                                (SprintType)sprint.Type, sprint.Location, sprint.ImageUrl, sprint.PromotionCode, sprint.IsTimeBased, sprint.DurationForTimeBasedEvent, sprint.DescriptionForTimeBasedEvent, sprint.Interval);
                            foreach (var participant in participants)
                            {
                                resultDto.AddParticipant(
                                    participant.User.Id,
                                    participant.User.Name,
                                    participant.User.ProfilePicture,
                                    participant.User.City,
                                    participant.User.Country,
                                    participant.User.CountryCode,
                                    participant.User.ColorCode,
                                    false,
                                    ParticipantStage.JOINED,
                                    friends.Contains(participant.User.Id));

                            }

                            resultDto.SprintInfo.IsProgramSprint = sprint.ProgramId > 0 ? true:false;
                            resultDto.SprintInfo.ProgramId = sprint.ProgramId;
                            sprintDto.Add(resultDto);
                        }
                    }
                }

                List<PublicSprintWithParticipantsDto> result = null;
                if (pageNo == 0 && limit == 0)
                {
                    result = sprintDto;
                }
                else
                {
                    result = sprintDto.Skip(pageNo * limit).Take(limit).ToList();
                }

                return result;
            }
            catch (Exception ex)
            { throw ex; }

        }

        public async Task<List<ReportItemDto>> GetReport(string timespan)
        {
            var reportData = await this.SprintRepo.GetReport(timespan);
            return reportData;
        }

        /// <summary>
        /// Validate Private Sprint Count For User
        /// </summary>
        /// <param name="userId"> creator id </param>
        /// <param name="lapsTime"> laps Time </param>
        /// <param name="privateSprintCount"> Limit of Private sprints </param>
        /// <returns></returns>
        public async Task<bool> ValidatePrivateSprintCountForUser(int userId, int lapsTime, int privateSprintCount)
        {

            var sprints = await this.SprintRepo.GetAllPrivateSprintsByUser(userId, lapsTime);
            if (sprints != null)
            {
                if (sprints.Count >= privateSprintCount)
                    return false;
                else
                    return true;

            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get All Images
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllImages()
        {
            Dictionary<string, string> imageUrlList = new Dictionary<string, string>();
            imageUrlList.Add("Image_1", "http://tiles.sprintcrowd.com/0001.jpg");
            imageUrlList.Add("Image_2", "http://tiles.sprintcrowd.com/0002.jpg");
            imageUrlList.Add("Image_3", "http://tiles.sprintcrowd.com/0003.jpg");
            imageUrlList.Add("Image_4", "http://tiles.sprintcrowd.com/0004.jpg");
            imageUrlList.Add("Image_5", "http://tiles.sprintcrowd.com/0005.jpg");
            imageUrlList.Add("Image_6", "http://tiles.sprintcrowd.com/0006.jpg");
            imageUrlList.Add("Image_7", "http://tiles.sprintcrowd.com/0007.jpg");
            imageUrlList.Add("Image_8", "http://tiles.sprintcrowd.com/0008.jpg");
            imageUrlList.Add("Image_9", "http://tiles.sprintcrowd.com/0009.jpg");
            imageUrlList.Add("Image_10", "http://tiles.sprintcrowd.com/0010.jpg");
            imageUrlList.Add("Image_11", "http://tiles.sprintcrowd.com/0011.jpg");
            imageUrlList.Add("Image_12", "http://tiles.sprintcrowd.com/0012.jpg");
            imageUrlList.Add("Image_13", "http://tiles.sprintcrowd.com/0013.jpg");
            imageUrlList.Add("Image_14", "http://tiles.sprintcrowd.com/0014.jpg");
            imageUrlList.Add("Image_15", "http://tiles.sprintcrowd.com/0015.jpg");
            imageUrlList.Add("Image_16", "http://tiles.sprintcrowd.com/0016.jpg");
            imageUrlList.Add("Image_17", "http://tiles.sprintcrowd.com/0017.jpg");
            imageUrlList.Add("Image_18", "http://tiles.sprintcrowd.com/0018.jpg");
            imageUrlList.Add("Image_19", "http://tiles.sprintcrowd.com/0019.jpg");
            imageUrlList.Add("Image_20", "http://tiles.sprintcrowd.com/0020.jpg");

            return imageUrlList;
        }

        /// <summary>
        /// Get All User Mails
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserMailReportDto>> GetAllUserMails()
        {
            try
            {
                return await this.userRepo.GetAllEmailUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create New Sprint Program
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sprintProgramDto"></param>
        /// <returns></returns>
        public async Task<SprintProgramDto> CreateNewSprintProgram(User user, SprintProgramDto sprintProgramDto)
        {

            SprintProgram sprintProgram = new SprintProgram();

            //if (sprintProgramDto.ProgramCode != null && sprintModel.promotionCode != string.Empty)
            //{
            //    Sprint sprintPromoCode = await this.userRepo.IsPromoCodeExist(sprintModel.promotionCode);
            //    if (sprintPromoCode != null)
            //    {
            //        throw new SCApplicationException((int)SprintErrorCode.AlreadyExistPromoCode, "Already exist promotion Code");
            //    }
            //}

            sprintProgram.Name = sprintProgramDto.Name;
            sprintProgram.Description = sprintProgramDto.Description;
            sprintProgram.Duration = sprintProgramDto.Duration;
            sprintProgram.IsPrivate = sprintProgramDto.IsPrivate;
            sprintProgram.ImageUrl = sprintProgramDto.ImageUrl;
            sprintProgram.GetSocialLink = string.Empty;
            sprintProgram.ProgramCode = sprintProgramDto.IsPrivate ? await this.generatePromotionCode(true) : null;
            sprintProgram.StartDate = sprintProgramDto.StartDate;
            sprintProgram.CreatedBy = user;
            sprintProgram.PromotionalText = sprintProgramDto.PromotionalText;
            sprintProgram.IsPromoteInApp = sprintProgramDto.IsPromoteInApp;

            SprintProgram addedSprintProgram = await this.SprintRepo.AddSprintProgram(sprintProgram);
            var sprintList = await this.SprintRepo.GetAllSprintsInPrograms(addedSprintProgram.Id);
            var customData = new
            {
                campaign_name = "programshare",
                programId = addedSprintProgram.Id.ToString(),
                programnCode = addedSprintProgram.ProgramCode,
                name = addedSprintProgram.Name,
                duration = addedSprintProgram.Duration.ToString(),
                startDateTime = addedSprintProgram.StartDate.ToString(),
                description = addedSprintProgram.Description,
                imageLink = addedSprintProgram.ImageUrl,
                endDate = addedSprintProgram.StartDate.AddDays(addedSprintProgram.Duration * 7),
                events = sprintList.Count().ToString()
            };
            try
            {
                var socialLink = sprintProgram.IsPrivate ?
                await this.SocialShareService.updateTokenAndGetInvite(customData) : string.Empty;
                //await this.SocialShareService.GetSmartLink(new SocialLink()
                //{
                //    Name = addedSprintProgram.Name,
                //    Description = addedSprintProgram.Description,
                //    ImageUrl = addedSprintProgram.ImageUrl,
                //    CustomData = customData
                //});

                addedSprintProgram.GetSocialLink = socialLink;
                // await this.SprintRepo.UpdateSprintProgram(addedSprintProgram);
               var programSprintList = await this.SprintRepo.GetProgramSprintListByProgramId(sprintProgram.Id);
                return SprintProgramDtoMapper(await this.SprintRepo.UpdateSprintProgram(addedSprintProgram), programSprintList);

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static SprintProgramDto SprintProgramDtoMapper(SprintProgram sprintProgram , List<Sprint> programSprintList)
        {
            

            SprintProgramDto result = new SprintProgramDto(
                sprintProgram, programSprintList
                );
            return result;
        }


        /// <summary>
        /// Get Sprint Program Details By ProgramId
        /// </summary>
        /// <param name="sprintProgramId"></param>
        /// <returns></returns>
        public async Task<SprintProgramDto> GetSprintProgramDetailsByProgramId(int sprintProgramId)
        {
            try
            {
                var programSprintList = await this.SprintRepo.GetProgramSprintListByProgramId(sprintProgramId);
                return SprintProgramDtoMapper(await this.SprintRepo.GetSprintProgramDetailsByProgramId(sprintProgramId), programSprintList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Update Sprint Program
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sprintProgramDto"></param>
        /// <returns></returns>
        public async Task<SprintProgramDto> UpdateSprintProgram(User user, SprintProgramDto sprintProgramDto)
        {

            SprintProgram sprintProgram = await this.SprintRepo.GetSprintProgramDetailsByProgramId(sprintProgramDto.Id);

            
            
            sprintProgram.Name = sprintProgramDto.Name;
            sprintProgram.Description = sprintProgramDto.Description;
            sprintProgram.Duration = sprintProgramDto.Duration;
            sprintProgram.IsPrivate = sprintProgramDto.IsPrivate;
            sprintProgram.ImageUrl = sprintProgramDto.ImageUrl;
            sprintProgram.GetSocialLink = string.Empty;
            sprintProgram.ProgramCode = sprintProgramDto.ProgramCode;
            sprintProgram.StartDate = sprintProgramDto.StartDate;
            sprintProgram.CreatedBy = user;
            sprintProgram.IsPublish = sprintProgramDto.IsPublish;
            sprintProgram.PromotionalText = sprintProgramDto.PromotionalText;
            sprintProgram.IsPromoteInApp = sprintProgramDto.IsPromoteInApp;

            var sprintList = await this.SprintRepo.GetAllSprintsInPrograms(sprintProgram.Id);
            var customData = new
            {
                campaign_name = "programshare",
                programId = sprintProgram.Id.ToString(),
                programnCode = sprintProgram.ProgramCode,
                name = sprintProgram.Name,
                duration = sprintProgram.Duration.ToString(),
                startDateTime = sprintProgram.StartDate.ToString(),
                description = sprintProgram.Description,
                imageLink = sprintProgram.ImageUrl,
                endDate = sprintProgram.StartDate.AddDays(sprintProgram.Duration * 7),
                events = sprintList.Count().ToString()
            };
            try
            {
                var socialLink = sprintProgram.IsPrivate ?
                await this.SocialShareService.updateTokenAndGetInvite(customData) : string.Empty;
                
                sprintProgram.GetSocialLink = socialLink;
                // await this.SprintRepo.UpdateSprintProgram(addedSprintProgram);
                var programSprintList = await this.SprintRepo.GetProgramSprintListByProgramId(sprintProgram.Id);
                return SprintProgramDtoMapper(await this.SprintRepo.UpdateSprintProgram(sprintProgram), programSprintList);

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get All Sprint Programms
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<SprintProgramsPageDto> GetAllSprintProgramms(int userId, string searchTerm, int pageNo, int limit)
        {
            try
            {
                var result = await this.userRepo.GetUserRoleInfo(userId);
                return await this.SprintRepo.GetAllSprintProgramms(userId, searchTerm, pageNo, limit, result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get All Sprint Program For Dashboard
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<SprintProgramsDashboardDto> GetAllSprintProgramForDashboard(int pageNo, int limit , int userId)
        {
            try
            {
              var sprintProgram =   await this.SprintRepo.GetAllSprintProgramForDashboard( pageNo, limit);
                List<SprintProgramDto> sprintProgramDto = new List<SprintProgramDto>();
                int participantCount = 0;

                foreach (SprintProgram programs in sprintProgram.sPrograms.Skip(pageNo * limit).Take(limit))
                {
                    sprintProgramDto.Add(new SprintProgramDto(programs, await this.SprintRepo.GetAllSprintListByProgrammid(programs.Id),true, await this.GetAllProgramParticipantsCount(programs.Id), await this.IsUserJoinedProgram(programs.Id, userId)));
                }

                foreach (SprintProgram programs in sprintProgram.sPrograms)
                {
                    participantCount = participantCount + await this.GetAllProgramParticipantsCount(programs.Id);
                }
                return new SprintProgramsDashboardDto
                {
                    dbPrograms = sprintProgramDto,
                    totalItems = sprintProgram.totalItems,
                    totalParticipants = participantCount

                }; 
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get All Sprint Programms Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetAllSprintProgrammsCount(int userId)
        {
            try
            {
                return this.SprintRepo.GetAllSprintProgrammsCount(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Remove sprint program from Admin Panel
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task RemoveSprintProgram(int userId, int programId)
        {
            Expression<Func<SprintProgram, bool>> programPredicate = s => s.Id == programId;
            var program = await this.SprintRepo.GetSprintProgram(programPredicate);
            if (program == null)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotMatchingSprintWithId, "Sprint program not found with given id");
            }
            else if (program.CreatedBy.Id != userId)
            {
                throw new SCApplicationException((int)SprintErrorCode.NotAllowedOperation, "Only creator can delete program");
            }
            else
            {
                program.Status = (int)SprintProgramStatus.ARCHIVED;
                await this.SprintRepo.UpdateSprintProgram(program);
                this.SprintRepo.SaveChanges();
                
            }
        }

        /// <summary>
        /// Get Program Sprint List By Sprint StartDate
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetProgramSprintListBySprintStartDate(DateTime sprintStartDate)
        {
            try
            {
                return ToSprintProgramDictionary(this.SprintRepo.GetProgramSprintListBySprintStartDate(sprintStartDate).Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To Sprint Program Dictionary
        /// </summary>
        /// <param name="sprintPrograms"></param>
        /// <returns></returns>
        public static Dictionary<int, string> ToSprintProgramDictionary(List<SprintProgram> sprintPrograms)
        {
            var dictionary = new Dictionary<int, string>();
            if (sprintPrograms != null)
            {
                foreach (var programs in sprintPrograms)
                {
                    if(!dictionary.Values.Contains(programs.Name))
                    dictionary.Add(programs.Id, programs.Name);
                }
            }

            return dictionary;
        }


        /// <summary>
        /// Get All Scheduled Programs Detail
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<ProgramSprintScheduleEvents>> GetAllScheduledProgramsDetail(int programId, int pageNo, int limit)
        {
            try
            {

                return await this.SprintRepo.GetAllScheduledProgramsDetail(programId, pageNo, limit);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Get All Program Participants with program detail
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<ProgramParticipantsDto> GetAllProgramParticipants(int programId, int userId)
        {
            
            try
            {
                var sprints = await this.SprintRepo.GetAllSprintsInPrograms(programId);
                var sprintIds = sprints.Select(x => x.Id);
                Expression <Func<SprintParticipant, bool>> participantPredicate = null;
                
                participantPredicate = s => sprintIds.Contains(s.SprintId);

                List<SprintParticipant> participants =  await this.SprintRepo.GetProgramSprintsParticipants(participantPredicate);
                List < SprintCrowd.BackEnd.Domain.Sprint.Dtos.ParticipantInfoDto> ParticipantInfoDto = new List<SprintCrowd.BackEnd.Domain.Sprint.Dtos.ParticipantInfoDto>();
                
                foreach(SprintParticipant sprintParticipant in participants )
                {
                    if (!ParticipantInfoDto.Any(p => p.Id == sprintParticipant.User.Id))
                    {
                        ParticipantInfoDto.Add(new SprintCrowd.BackEnd.Domain.Sprint.Dtos.ParticipantInfoDto(
                            sprintParticipant.User.Id,
                            sprintParticipant.User.Name,
                            sprintParticipant.User.ProfilePicture,
                            sprintParticipant.User.City,
                            sprintParticipant.User.Country, sprintParticipant.User.CountryCode, sprintParticipant.User.ColorCode
                            , false, sprintParticipant.Stage, false));
                    }
                }
                
                var programDetail = await this.SprintRepo.GetSprintProgramDetailsByProgramId(programId);
                ProgramParticipantsDto programParticipantsDto = new ProgramParticipantsDto(programDetail.Name, programDetail.Description, programDetail.Duration, programDetail.StartDate , programDetail.StartDate.AddDays(programDetail.Duration*7),participants.Count, ParticipantInfoDto, await this.IsUserJoinedProgram(programId, userId),programDetail.IsPromoteInApp,programDetail.PromotionalText, sprintIds.Count(), ParticipantInfoDto.Count(), programDetail.IsPrivate);

                return programParticipantsDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///Get All Program Participants Count
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<int> GetAllProgramParticipantsCount(int programId)
        {

            try
            {
                var sprints = await this.SprintRepo.GetAllSprintsInPrograms(programId);
                var sprintIds = sprints.Select(x => x.Id);
                Expression<Func<SprintParticipant, bool>> participantPredicate = null;

                participantPredicate = s => sprintIds.Contains(s.SprintId);

                List<SprintParticipant> participants = await this.SprintRepo.GetProgramSprintsParticipants(participantPredicate);
               
                return participants.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get All Program Sprints Hosts
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task <List<UserDto>> GetAllProgramSprintsHosts(int programId)
        {

            try
            {
                var sprints = await this.SprintRepo.GetAllSprintsInPrograms(programId);
                var sprintIds = sprints.Select(x => x.Id);
                Expression<Func<SprintParticipant, bool>> participantPredicate = null;

                List<string> hosts = new List<string>(); 
                List<string> coHosts = new List<string>();
                hosts = sprints.Select(x => x.InfluencerEmail).Distinct().ToList();
                coHosts = sprints.Select(x => x.InfluencerEmailSecond).Distinct().ToList();
                hosts.AddRange(coHosts);
                List<UserDto> hostUsersList = new List<UserDto>();
                foreach (string email in hosts)
                {
                    if(email != null)
                    hostUsersList.Add(await this.UserService.getUserByEmail(email));
                }

                
                return hostUsersList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Join Program
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="userId"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public async Task<dynamic> JoinProgram(int programId, int userId,string programCode, bool isPrivateProgram)
        {
            bool success = false;
            SprintProgram program = new SprintProgram();
            ProgramParticipant proParticipatInfor = new ProgramParticipant();
            //if promoCode program
            if (isPrivateProgram && !string.IsNullOrEmpty(programCode))
            {
                program = await this.SprintRepo.GetSprintProgramDetailsByProgramCode(programCode);
                if (program != null)
                {
                    //To check the participant joined or not
                    proParticipatInfor = await this.SprintRepo.GetProgramByUserId(userId, program.Id);
                    programId = program.Id;

                    if (program.ProgramCode != programCode)
                    {
                        throw new Application.SCApplicationException((int)ErrorCodes.NotAllowedOperation, "Notallowed");
                    }
                }
            }
            else if (!isPrivateProgram && programId > 0)
            {
                program = await this.SprintRepo.GetSprintProgramDetailsByProgramId(programId);
                proParticipatInfor = await this.SprintRepo.GetProgramByUserId(userId, programId);

                if (program.IsPrivate)
                {
                    if (program.IsPrivate != isPrivateProgram || program.ProgramCode != programCode)
                    throw new Application.SCApplicationException((int)ErrorCodes.NotAllowedOperation, "Notallowed");
                }
            }
            else
            {
                throw new Application.SCApplicationException((int)ErrorCodes.NotAllowedOperation, "Notallowed");
            }
            
            if (program == null)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.SprintNotFound, "Program not found");
            }

            if (proParticipatInfor != null && proParticipatInfor.ProgramId == programId)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.AlreadyJoined, "Already joined");
            }
            
            //Get all sprints in the program
            var sprints = await this.SprintRepo.GetAllSprintsInPrograms(programId);              
            //Join the participant to program
            var programSprint = await this.SprintRepo.AddProgramParticipant(programId, userId);
            if (sprints != null && sprints.Count > 0)
            {
                foreach (Sprint sprint in sprints)
                {
                    await this.SprintParticipantRepo.AddSprintParticipant(sprint.Id, userId);
                }
            }
            if (programSprint != null)
                success = true;
            return success;

        }

        /// <summary>
        ///Get All Program Participants Count
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserJoinedProgram(int programId , int userId)
        {

            try
            {
                bool isJoined = false;
                var proParticipatInfor = await this.SprintRepo.GetProgramByUserId(userId, programId);
                if (proParticipatInfor != null && proParticipatInfor.Id.ToString() != string.Empty)
                    isJoined = true;
                return isJoined;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}