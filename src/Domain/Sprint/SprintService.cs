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
        public SprintService(ISprintRepo sprintRepo, INotificationClient notificationClient, IUserRepo _userRepo, ISocialShareService socialShareService, ISprintParticipantRepo sprintParticipantRepo)
        {
            this.SprintRepo = sprintRepo;
            this.NotificationClient = notificationClient;
            this.userRepo = _userRepo;
            this.SocialShareService = socialShareService;
            this.SprintParticipantRepo = sprintParticipantRepo;
        }
        private readonly IUserRepo userRepo;

        private ISocialShareService SocialShareService { get; }
        private ISprintRepo SprintRepo { get; }
        private INotificationClient NotificationClient { get; }

        private ISprintParticipantRepo SprintParticipantRepo { get; }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="searchTerm">Search term to filter</param>
        /// <param name="sortBy">Sort to filter</param>
        /// <param name="filterBy">Term to filter</param>
        /// <returns>Available all events</returns>
        public async Task<List<Sprint>> GetAll(int eventType, string searchTerm, string sortBy, string filterBy)
        {

            // List<Sprint> allSprints = new List<Sprint>();

            //allSprints = await this.SprintRepo.GetAllEvents(eventType, searchTerm, sortBy, filterBy);

            //for (int i = 0; i < allSprints.Count; i++)
            //{
            //    if (allSprints[i].InfluencerEmail != null)
            //    {
            //        allSprints[i].InfluencerEmail = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(allSprints[i].InfluencerEmail);
            //    }
            //}

            return await this.SprintRepo.GetAllEvents(eventType, searchTerm, sortBy, filterBy);
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

            if (!string.IsNullOrEmpty(sprintModel.InfluencerEmail) && !string.Equals(sprintAavail.InfluencerEmail, sprintModel.InfluencerEmail))
            {
                string encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmail);
                sprintAavail.InfluencerEmail = encryptedEamil;
                sprintAavail.InfluencerAvailability = true;
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
                sprint.InfluencerEmail);
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

        public async Task<String> generatePromotionCode()
        {
            int x = 0;
            Sprint lastSpecialSprint = await this.SprintRepo.GetLastSpecialSprint();
            if (Int32.TryParse(lastSpecialSprint.PromotionCode, out x))
            {
                int promocode = Int32.Parse(lastSpecialSprint.PromotionCode) + 1;
                string strPromocode = promocode.ToString("D6");
                return strPromocode;
            }
            string iniPromoCode = x.ToString("D6");
            return iniPromoCode;
        }

        public async Task<CreateSprintDto> CreateNewSprint(
            User user,
            CreateSprintModel sprintModel,
            TimeSpan durationForTimeBasedEvent,
            string descriptionForTimeBasedEvent)
        {

            Sprint sprint = new Sprint();

            if (!string.IsNullOrEmpty(sprintModel.InfluencerEmail))
            {
                if (!StringUtils.IsBase64String(sprintModel.InfluencerEmail))
                {
                    sprint.InfluencerEmail = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(sprintModel.InfluencerEmail);
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
            sprint.Distance = sprintModel.Distance;
            sprint.StartDateTime = sprintModel.StartTime;
            sprint.CreatedBy = user;
            sprint.IsNarrationsOn = sprintModel.IsNarrationsOn;
            sprint.Type = sprintModel.SprintType;
            sprint.NumberOfParticipants = sprintModel.NumberOfParticipants == null ? NumberOfParticipants(sprintModel.SprintType) : (int)sprintModel.NumberOfParticipants;
            sprint.InfluencerAvailability = sprintModel.InfluencerAvailability;
            sprint.InfluencerEmail = sprintModel.InfluencerEmail;
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
                string userGroup = await this.SprintParticipantRepo.GetUserGroupName(addedSprint.Id, user.Id);
                await this.SprintRepo.AddParticipant(user.Id, addedSprint.Id, userGroup, ParticipantStage.JOINED);
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
                var customData = new { campaign_name = "sprintshare", sprintId = sprint.Id.ToString(), promotionCode = sprint.PromotionCode };

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
                await this.SprintRepo.UpdateSprint(sprint);
            }

            return CreateSprintDtoMapper(sprint, user);
        }

        /// <summary>
        /// Create multiple sprints
        /// </summary>
        public async Task CreateMultipleSprints(
            User user,
            string name,
            int distance,
            DateTime startTime,
            int type,
            int? numberOfParticipants,
            string infulenceEmail,
            int draft,
            bool influencerAvailability,
            string repeatType)
        {

            if (infulenceEmail != null)
            {
                var email = infulenceEmail;
                var encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);

                infulenceEmail = encryptedEamil;
            }
            //var decryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(encryptedEamil);

            List<Sprint> recurrentSprints = new List<Sprint>();
            DateTime endDate = startTime.AddMonths(3);
            int incementalSprintNumber = 0;

            if (repeatType == "DAILY")
            {
                while (startTime <= endDate)
                {
                    Sprint sprint = new Sprint();
                    if (draft == 0)
                    {
                        sprint.Name = name;
                        sprint.Distance = distance;
                        sprint.StartDateTime = startTime;
                        sprint.CreatedBy = user;
                        sprint.Type = type;
                        sprint.Status = (int)SprintStatus.NOTSTARTEDYET;
                        sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                        sprint.InfluencerAvailability = influencerAvailability;
                        sprint.InfluencerEmail = infulenceEmail;
                        sprint.DraftEvent = draft;
                    }
                    else
                    {
                        sprint.Name = name;
                        sprint.Distance = distance;
                        sprint.StartDateTime = startTime;
                        sprint.CreatedBy = user;
                        sprint.Type = type;
                        sprint.Status = (int)SprintStatus.NOTPUBLISHEDYET;
                        sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                        sprint.InfluencerAvailability = influencerAvailability;
                        sprint.InfluencerEmail = infulenceEmail;
                        sprint.DraftEvent = draft;
                    }
                    recurrentSprints.Add(sprint);
                    incementalSprintNumber++;
                    if (incementalSprintNumber != 1)
                    {
                        name = name.Split(new char[] { '(', ')' })[0];
                    }
                    name = name + "(" + incementalSprintNumber + ")";
                    startTime = startTime.AddDays(1);
                }
            }
            else if (repeatType == "WEEKLY")
            {
                while (startTime <= endDate)
                {
                    Sprint sprint = new Sprint();
                    if (draft == 0)
                    {
                        sprint.Name = name;
                        sprint.Distance = distance;
                        sprint.StartDateTime = startTime;
                        sprint.CreatedBy = user;
                        sprint.Type = type;
                        sprint.Status = (int)SprintStatus.NOTSTARTEDYET;
                        sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                        sprint.InfluencerAvailability = influencerAvailability;
                        sprint.InfluencerEmail = infulenceEmail;
                        sprint.DraftEvent = draft;
                    }
                    else
                    {
                        sprint.Name = name;
                        sprint.Distance = distance;
                        sprint.StartDateTime = startTime;
                        sprint.CreatedBy = user;
                        sprint.Type = type;
                        sprint.Status = (int)SprintStatus.NOTPUBLISHEDYET;
                        sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                        sprint.InfluencerAvailability = influencerAvailability;
                        sprint.InfluencerEmail = infulenceEmail;
                        sprint.DraftEvent = draft;
                    }
                    recurrentSprints.Add(sprint);
                    incementalSprintNumber++;
                    if (incementalSprintNumber != 1)
                    {
                        name = name.Split(new char[] { '(', ')' })[0];
                    }
                    name = name + "(" + incementalSprintNumber + ")";
                    startTime = startTime.AddDays(7);
                }
            }
            else if (repeatType == "MONTHLY")
            {
                while (startTime <= endDate)
                {
                    Sprint sprint = new Sprint();
                    if (draft == 0)
                    {
                        sprint.Name = name;
                        sprint.Distance = distance;
                        sprint.StartDateTime = startTime;
                        sprint.CreatedBy = user;
                        sprint.Type = type;
                        sprint.Status = (int)SprintStatus.NOTSTARTEDYET;
                        sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                        sprint.InfluencerAvailability = influencerAvailability;
                        sprint.InfluencerEmail = infulenceEmail;
                        sprint.DraftEvent = draft;
                    }
                    else
                    {
                        sprint.Name = name;
                        sprint.Distance = distance;
                        sprint.StartDateTime = startTime;
                        sprint.CreatedBy = user;
                        sprint.Type = type;
                        sprint.Status = (int)SprintStatus.NOTPUBLISHEDYET;
                        sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
                        sprint.InfluencerAvailability = influencerAvailability;
                        sprint.InfluencerEmail = infulenceEmail;
                        sprint.DraftEvent = draft;
                    }
                    recurrentSprints.Add(sprint);
                    incementalSprintNumber++;
                    if (incementalSprintNumber != 1)
                    {
                        name = name.Split(new char[] { '(', ')' })[0];
                    }
                    name = name + "(" + incementalSprintNumber + ")";
                    startTime = startTime.AddMonths(1);
                }
            }
            await this.SprintRepo.AddMultipleSprints(recurrentSprints);
            this.SprintRepo.SaveChanges();
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
                pariticipants = this.SprintRepo.GetParticipants(participantPredicate).ToList(); ;
            }
            else
            {
                pariticipants = this.SprintRepo.GetParticipants(participantPredicate).Skip(pageNo * limit).Take(limit).ToList();
            }




            User influencer = null;
            if (sprint.Type == (int)SprintType.PublicSprint && sprint.InfluencerAvailability)
            {
                influencer = await this.SprintRepo.FindInfluencer(sprint.InfluencerEmail);
                if (influencer == null)
                    influencer = await this.SprintRepo.FindInfluencer(Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(sprint.InfluencerEmail));
            }
            return SprintWithPariticpantsMapper(sprint, pariticipants.ToList(), influencer);
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



            return this.SprintRepo.GetParticipants(participantPredicate).Skip(pageNo * limit).Take(limit).ToList();
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
            string userGroup = await this.SprintParticipantRepo.GetUserGroupName(sprintId, inviteeId);
            await this.SprintRepo.AddParticipant(inviteeId, sprintId, userGroup);
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

        public static SprintWithPariticpantsDto SprintWithPariticpantsMapper(Sprint sprint, List<SprintParticipant> participants, User influencer = null)
        {
            SprintWithPariticpantsDto result = new SprintWithPariticpantsDto(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.NumberOfParticipants,
                sprint.StartDateTime,
                (SprintType)sprint.Type,
                sprint.Location,
                sprint.PromotionCode,
                sprint.IsNarrationsOn);
            participants
                .ForEach(p =>
                {
                    var isInfulencer = false;
                    if (influencer != null)
                    {
                        isInfulencer = influencer.Id == p.UserId;
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
                        p.UserGroup,
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
                openEvents = this.SprintRepo.GetSprint_Open(query).OrderBy(x => x.StartDateTime);

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
                                   (SprintType)sprint.Type, sprint.Location, sprint.ImageUrl, sprint.PromotionCode);
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
                                (SprintType)sprint.Type, sprint.Location, sprint.ImageUrl, sprint.PromotionCode);
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

    }
}