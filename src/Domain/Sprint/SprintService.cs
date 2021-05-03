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
    using SprintCrowd.BackEnd.Domain.Sprint.Video;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SocialShare;
    using SprintCrowd.BackEnd.Web.SocialShare;

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
        public SprintService(ISprintRepo sprintRepo, INotificationClient notificationClient, IUserRepo _userRepo, ISocialShareService socialShareService)
        {
            this.SprintRepo = sprintRepo;
            this.NotificationClient = notificationClient;
            this.userRepo = _userRepo;
            this.SocialShareService = socialShareService;
        }
        private readonly IUserRepo userRepo;

        private ISocialShareService SocialShareService { get; }
        private ISprintRepo SprintRepo { get; }
        private INotificationClient NotificationClient { get; }

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
            string name,
            int? distance,
            DateTime? startTime,
            int? numberOfParticipants,
            bool influencerAvailability,
            string influencerEmail,
            int? draftEvent,
            string imageUrl,
            VideoType videoType,
            String videoLink,
            string promotionCode,
            bool isTimeBased,
            TimeSpan durationForTimeBasedEvent,
            string descriptionForTimeBasedEvent)
        {

            if (promotionCode != null && promotionCode != string.Empty)
            {
                Sprint sprintPromoCode = await this.userRepo.IsPromoCodeExist(promotionCode);
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
            string oldName = sprintAavail.Name;
            if (name != String.Empty)
            {
                sprintAavail.Name = name;
            }
            if (distance != null)
            {
                sprintAavail.Distance = (int)distance;
            }
            if (startTime != null)
            {
                sprintAavail.StartDateTime = (DateTime)startTime;
            }
            if (distance != null)
            {
                sprintAavail.Distance = (int)distance;
            }
            if (numberOfParticipants != null)
            {
                sprintAavail.NumberOfParticipants = (int)numberOfParticipants;
            }

            sprintAavail.InfluencerAvailability = influencerAvailability;

            if (!string.IsNullOrEmpty(influencerEmail) && !string.Equals(sprintAavail.InfluencerEmail, influencerEmail))
            {
                string encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(influencerEmail);
                sprintAavail.InfluencerEmail = encryptedEamil;
                sprintAavail.InfluencerAvailability = true;
            }

            if (draftEvent != null)
            {
                sprintAavail.DraftEvent = (int)draftEvent;

                if (draftEvent == 0)
                {
                    sprintAavail.Status = (int)SprintStatus.NOTSTARTEDYET;
                }
                else
                {
                    sprintAavail.Status = (int)SprintStatus.NOTPUBLISHEDYET;
                }
            }
            sprintAavail.ImageUrl = imageUrl;
            sprintAavail.VideoLink = videoLink;
            sprintAavail.VideoType = videoType;
            sprintAavail.PromotionCode = promotionCode;
            sprintAavail.IsTimeBased = isTimeBased;
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
        public async Task<CreateSprintDto> CreateNewSprint(
            User user,
            string name,
            int distance,
            bool isSmartInvite,
            DateTime startTime,
            int type,
            int? numberOfParticipants,
            string infulenceEmail,
            int draft,
            bool influencerAvailability,
            string imageUrl,
            VideoType videoType,
            String videoLink,
            string promotionCode,
            bool isTimeBased,
            TimeSpan durationForTimeBasedEvent,
            string descriptionForTimeBasedEvent)
        {

            if (!string.IsNullOrEmpty(infulenceEmail))
            {
                if (!StringUtils.IsBase64String(infulenceEmail))
                {
                    infulenceEmail = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(infulenceEmail);
                }
            }

            if (promotionCode != null && promotionCode != string.Empty)
            {
                Sprint sprintPromoCode = await this.userRepo.IsPromoCodeExist(promotionCode);
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

            Sprint sprint = new Sprint();

            sprint.SocialMediaLink = string.Empty;
            sprint.Name = name;
            sprint.Distance = distance;
            sprint.StartDateTime = startTime;
            sprint.CreatedBy = user;
            sprint.Type = type;
            sprint.NumberOfParticipants = numberOfParticipants == null ? NumberOfParticipants(type) : (int)numberOfParticipants;
            sprint.InfluencerAvailability = influencerAvailability;
            sprint.InfluencerEmail = infulenceEmail;
            sprint.DraftEvent = draft;
            sprint.ImageUrl = imageUrl;
            sprint.PromotionCode = promotionCode == "PROMO" ? DateUtils.RandomString(2) + DateUtils.getNowShortTimeStamp() : null;
            sprint.IsSmartInvite = isSmartInvite;
            sprint.IsTimeBased = isTimeBased;
            sprint.DurationForTimeBasedEvent = durationForTimeBasedEvent;
            sprint.DescriptionForTimeBasedEvent = descriptionForTimeBasedEvent;
            sprint.VideoLink = videoLink;
            sprint.VideoType = videoType;

            if (draft == 0)
            {
                sprint.Status = (int)SprintStatus.NOTSTARTEDYET;

            }
            else
            {
                sprint.Status = (int)SprintStatus.NOTPUBLISHEDYET;
            }

            Sprint addedSprint = await this.SprintRepo.AddSprint(sprint);
            if (type == (int)SprintType.PrivateSprint)
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

            if (draft == 0)
            {
                var customData = new { campaign_name = "sprintshare", sprintId = sprint.Id.ToString(), promotionCode = sprint.PromotionCode };

                var socialLink = isSmartInvite ?
                await this.SocialShareService.updateTokenAndGetInvite(customData) :
                await this.SocialShareService.GetSmartLink(new SocialLink()
                {
                    Name = name,
                    Description = descriptionForTimeBasedEvent,
                    ImageUrl = imageUrl,
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
        /// <returns><see cref="SprintWithPariticpantsDto">sprint details</see></returns>
        public async Task<SprintWithPariticpantsDto> GetSprintWithPaticipants(int sprintId)
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
            var pariticipants = this.SprintRepo.GetParticipants(participantPredicate);
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
        /// <returns></returns>
        public async Task<List<SprintParticipant>> GetSprintPaticipants(int sprintId, int pageNo, int limit)
        {
            Expression<Func<SprintParticipant, bool>> participantPredicate = s =>
               s.SprintId == sprintId && s.User.Name != string.Empty;

            return this.SprintRepo.GetParticipants(participantPredicate).OrderByDescending(d => d.FinishTime).Skip(pageNo).Take(limit).ToList();
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
                sprint.PromotionCode);
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

        public async Task<List<PublicSprintWithParticipantsDto>> GetOpenEvents(int userId, int timeOffset)
        {

            try
            {
                var userPreference = await this.SprintRepo.GetUserPreference(userId);
                var query = new PublicSprintQueryBuilder(userPreference).BuildOpenEvents(timeOffset);
                IEnumerable<Sprint> openEvents = await this.SprintRepo.GetSprints(query);
                var sprintDto = new List<PublicSprintWithParticipantsDto>();
                var friendsRelations = this.SprintRepo.GetFriends(userId);
                var friends = friendsRelations.Select(f => f.AcceptedUserId == userId ? f.SharedUserId : f.AcceptedUserId);
                foreach (var sprint in openEvents)
                {
                    if (sprint.PromotionCode == null || sprint.PromotionCode == string.Empty)
                    {
                        var participants = sprint.Participants.Where(s =>
                            s.User.UserState == UserState.Active &&
                            s.Stage != ParticipantStage.DECLINE && s.Stage != ParticipantStage.QUIT);
                        if (!participants.Any(p => p.Id == userId))
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
                return sprintDto;
            }
            catch(Exception ex)
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