namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.SprintManager;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
    using SprintCrowdBackEnd.Web.Sprint.Models;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Common;
    using Serilog;
    using SprintCrowd.BackEnd.Utils;
    using SprintCrowdBackEnd.Domain.SprintParticipant.Dtos;

    /// <summary>
    /// Implements ISprintParticipantService interface for hanle sprint participants
    /// </summary>
    public class SprintParticipantService : ISprintParticipantService
    {
        /// <summary>
        /// Initalize SprintParticipantService class
        /// </summary>
        /// <param name="sprintParticipantRepo">sprint participant repository</param>
        /// <param name="notificationClient">notification client</param>
        public SprintParticipantService(ISprintParticipantRepo sprintParticipantRepo, INotificationClient notificationClient, IUserRepo userRepo)
        {
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.NotificationClient = notificationClient;
            this._userRepo = userRepo;
        }

        private readonly IUserRepo _userRepo;
        public SprintParticipantService(ISprintParticipantRepo sprintParticipantRepo, INotificationClient notificationClient)
        {
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.NotificationClient = notificationClient;

        }
        private ISprintParticipantRepo SprintParticipantRepo { get; }

        private INotificationClient NotificationClient { get; }

        /// <summary>
        /// Mark the attendece for the given sprint and notify with evnet
        /// EventName.MarkedAttenence with MarkAttendenceMessage message
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for for participant</param>
        public async Task MarkAttendence(int sprintId, int userId)
        {
            //Check whether Influencer sprint or not
            bool IsIinfluencerEventParticipant = false;
            var sprint = await this.SprintParticipantRepo.GetSprint(sprintId);
            if (sprint.Type == (int)SprintType.PublicSprint && sprint.InfluencerAvailability)
                IsIinfluencerEventParticipant = true;

            var result = await this.SprintParticipantRepo.MarkAttendence(sprintId, userId, IsIinfluencerEventParticipant);
            Console.WriteLine("MarkAttendence service Result" + result.Name + "Sprint ID " + sprintId);
            var participatInfor = await this.SprintParticipantRepo.GetByUserIdSprintId(userId, sprintId);
            this.NotificationClient.SprintNotificationJobs.SprintMarkAttendace(
                sprintId,
                userId,
                result.Name,
                result.ProfilePicture,
                result.Country,
                result.CountryCode,
                result.City,
                result.ColorCode);
            this.SprintParticipantRepo.SaveChanges();
            return;
        }

        /// <summary>
        /// Join user for a sprint
        /// </summary>
        /// <param name="sprintId">sprint id going to join</param>
        /// <param name="userId">user id who going to join</param>
        /// <param name="notificationId"> notification id</param>
        /// <param name="accept">accept or decline</param>
        public async Task<ParticipantInfoDto> JoinSprint(int sprintId, int userId, int notificationId, bool accept = true)
        {
            var sprint = await this.SprintParticipantRepo.GetSprint(sprintId);
            if (sprint == null)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.SprintNotFound, "Sprint not found");
            }
            if (sprint != null && sprint.StartDateTime.AddMinutes(sprint.Interval) < DateTime.UtcNow)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.SprintExpired, "Sprint Expired");
            }
            else
            {
                var numberOfParticipants = this.SprintParticipantRepo.GetParticipantCount(sprintId);
                if (sprint.NumberOfParticipants <= numberOfParticipants)
                {
                    throw new Application.SCApplicationException((int)ErrorCodes.MaxUserExceeded, "Maximum user exceeded");
                }
            }

            Expression<Func<SprintParticipant, bool>> query = s => s.UserId == userId && s.SprintId == sprintId && s.User.UserState == UserState.Active;
            var inviteUser = await this.SprintParticipantRepo.Get(query);
            var creator = this.SprintParticipantRepo.GetCreator(sprintId);

            if (sprint.Type == (int)SprintType.PrivateSprint)
            {
                if (inviteUser == null)
                {
                    throw new Application.SCApplicationException((int)ErrorCodes.NotFounInvitation, "Not found invitation");
                }
                else if (userId != creator.Id && inviteUser.Stage != ParticipantStage.PENDING)
                {
                    throw new Application.SCApplicationException((int)ErrorCodes.AlreadyJoined, "Already joined for an event");
                }
                else
                {
                    if (accept)
                    {
                        await this.SprintParticipantRepo.JoinSprint(userId, sprintId, sprint.Type);
                    }
                    else
                    {
                        await this.SprintParticipantRepo.DeleteParticipant(userId, sprintId);
                    }

                    await this.SprintParticipantRepo.RemoveNotification(notificationId);
                    this.NotificationClient.SprintNotificationJobs.SprintJoin(
                        sprint.Id,
                        sprint.Name,
                        (SprintType)sprint.Type,
                        inviteUser.User.Id,
                        inviteUser.User.Name,
                        inviteUser.User.ProfilePicture,
                        accept);
                }
            }
            else
            {
                if (inviteUser != null && (inviteUser.Stage == ParticipantStage.JOINED || inviteUser.Stage == ParticipantStage.MARKED_ATTENDENCE))
                {
                    throw new Application.SCApplicationException((int)ErrorCodes.AlreadyJoined, "Already joined for an event");
                }
                if (sprint.PromotionCode == string.Empty)
                {
                    if (inviteUser != null)
                    {
                        await this.SprintParticipantRepo.JoinSprint(userId, sprintId, sprint.Type);
                        this.NotificationClient.SprintNotificationJobs.SprintJoin(
                            sprint.Id,
                            sprint.Name,
                            (SprintType)sprint.Type,
                            inviteUser.User.Id,
                            inviteUser.User.Name,
                            inviteUser.User.ProfilePicture,
                            accept);
                    }
                    else
                    {
                        var joinedUser = await this.SprintParticipantRepo.AddSprintParticipant(sprintId, userId);
                        this.NotificationClient.SprintNotificationJobs.SprintJoin(
                            sprint.Id,
                            sprint.Name,
                            (SprintType)sprint.Type,
                            joinedUser.User.Id,
                            joinedUser.User.Name,
                            joinedUser.User.ProfilePicture,
                            accept);
                    }

                }
                else
                {
                    var joinedUser = await this.SprintParticipantRepo.AddSprintParticipant(sprintId, userId);
                    await this.SprintParticipantRepo.JoinSprint(userId, sprintId, sprint.Type);

                    this.NotificationClient.SprintNotificationJobs.SprintJoin(
                            sprint.Id,
                            sprint.Name,
                            (SprintType)sprint.Type,
                            joinedUser.User.Id,
                            joinedUser.User.Name,
                            joinedUser.User.ProfilePicture,
                            accept);
                }
            }

            var user = await this.SprintParticipantRepo.GetParticipant(userId);
            var userDto = new ParticipantInfoDto(user.Id, user.Name, user.ProfilePicture, user.Code, user.ColorCode, user.City, user.Country, user.CountryCode, ParticipantStage.JOINED, creator.Id == userId);
            this.SprintParticipantRepo.SaveChanges();
            return userDto;

        }

        /// <summary>
        /// Update User Country Detail By UserId
        /// </summary>
        /// <param name="userCountryInfo"></param>
        /// <returns></returns>
        public bool UpdateUserCountryDetailByUserId(UserCountryDetail userCountryInfo)
        {
            bool success = false;
            success = this.SprintParticipantRepo.UpdateCountryByUserId(userCountryInfo) > 0 ? true : false;
            return success;
        }

        /// <summary>
        /// Exit sprint which join for event
        /// </summary>
        /// <param name="sprintId ">exit sprint id</param>
        /// <param name="userId ">user id which leaving the event</param>
        /// <returns><see cref="ExitSprintResult "> Exist sprint result</see></returns>
        // TODO : notification
        public async Task<ExitSprintResult> ExitSprint(int sprintId, int userId, int distance,string raceCompletedDuation)
        {
            try
            {
                Expression<Func<SprintParticipant, bool>> participantQuery = p => p.UserId == userId && p.SprintId == sprintId && p.User.UserState == UserState.Active;
                var participant = await this.SprintParticipantRepo.Get(participantQuery);

                 
                if (participant.Stage != ParticipantStage.COMPLETED)
                {
                    if (participant.Sprint.IsTimeBased)
                    {
                        participant.DistanceRan = distance;
                        participant.RaceCompletedDuration = raceCompletedDuation;
                    }
                    //Mark Attendance users only can exit
                    if(participant.Stage == ParticipantStage.MARKED_ATTENDENCE)
                    participant.Stage = ParticipantStage.QUIT;
                    participant.FinishTime = DateTime.UtcNow;
                    
                }
                this.SprintParticipantRepo.SaveChanges();
                this.NotificationClient.SprintNotificationJobs.SprintExit(
                    participant.SprintId,
                    participant.Sprint.Name,
                    participant.Sprint.IsTimeBased? distance : participant.Sprint.Distance,
                    participant.Sprint.StartDateTime,
                    participant.Sprint.NumberOfParticipants,
                    (SprintStatus)participant.Sprint.Status,
                    (SprintType)participant.Sprint.Type,
                    participant.Sprint.CreatedBy.Id,
                    participant.UserId,
                    (int)participant.Stage,
                    participant.User.Name,
                    participant.User.ProfilePicture,
                    participant.User.Code,
                    participant.User.City,
                    participant.User.Country,
                    participant.User.CountryCode);

                return new ExitSprintResult { Result = ExitResult.Success };
            }
            catch (Exception ex)
            {
                return new ExitSprintResult { Result = ExitResult.Faild, Reason = ex.Message.ToString() };
            }
        }

        /// <summary>
        /// Get all pariticipant with given stage <see cref="ParticipantStage"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="ParticipantInfoDto"> list of participant info</see></returns>
        public async Task<List<ParticipantInfoDto>> GetParticipants(int sprintId, ParticipantStage stage)
        {
            var joinedParticipants = await this.SprintParticipantRepo.GetParticipants(sprintId, stage);
            List<ParticipantInfoDto> participantInfos = new List<ParticipantInfoDto>();
            joinedParticipants.ForEach(p =>
            {
                var participant = new ParticipantInfoDto(
                    p.User.Id,
                    p.User.Name,
                    p.User.ProfilePicture,
                    p.User.Code,
                    p.User.ColorCode,
                    p.User.City,
                    p.User.Country,
                    p.User.CountryCode,
                    p.Stage,
                    p.User.Id == p.Sprint.CreatedBy.Id);
                participantInfos.Add(participant);
            });
            return participantInfos;
        }

        /// <summary>
        /// Get all sprint info with given filters
        /// Change request 12/11/2019 Mobile application event start now tab require user already created event
        /// reguradless 24H, for easyness change this API to send creator event embedded with sprints
        /// @todo remove this change and handle this in mobile side
        /// </summary>
        /// <param name="userId">participant id</param>
        /// <param name="sprintType"><see cref="SprintType"> sprint type</see></param>
        /// <param name="stage"><see cref="ParticipantStage"> participant stage</see></param>
        /// <param name="distanceFrom">distance in meters from</param>
        /// <param name="distanceTo">distance in meters from</param>
        /// <param name="startFrom">start from time in minutes</param>
        /// <param name="currentTimeBuff">current time difference</param>
        /// <returns><see cref="SprintInfo"> sprint info </see> </returns>
        public async Task<List<GetCommonSprintDto>> GetSprints(
            int userId,
            SprintType? sprintType,
            ParticipantStage? stage,
            int? distanceFrom,
            int? distanceTo,
            int? startFrom,
            int? currentTimeBuff)
        {
            var time = DateTime.UtcNow;
            var now = DateTime.UtcNow;
            if (currentTimeBuff != null)
            {
                now = DateTime.UtcNow.AddMinutes((int)currentTimeBuff);
            }

            if (startFrom != null)
            {
                time = time.AddHours((int)startFrom);
            }

            //Expression<Func<SprintParticipant, bool>> query = s =>
            //    s.UserId == userId &&
            //    s.User.UserState == UserState.Active &&
            //    s.Sprint.CreatedBy.Id != userId &&
            //    (s.Sprint.Type == (int)sprintType || sprintType == null) &&
            //    (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
            //    (s.Sprint.Status != (int)SprintStatus.ARCHIVED) &&
            //    (s.Sprint.Distance >= distanceFrom || distanceFrom == 0) &&
            //    (s.Sprint.Distance <= distanceTo || distanceTo == 0) &&
            //    ((s.Sprint.StartDateTime <= time && s.Sprint.StartDateTime > now) || startFrom == 0) &&
            //    (s.Sprint.StartDateTime > now);

            Expression<Func<SprintParticipant, bool>> creatorQueryCommon = s =>
               s.UserId == userId &&
               s.User.UserState == UserState.Active &&
               (s.Sprint.Type == (int)sprintType || sprintType == null) &&
               (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
               s.Sprint.Status != (int)SprintStatus.ARCHIVED &&
               (s.Sprint.Distance >= distanceFrom || distanceFrom == 0) &&
               (s.Sprint.Distance <= distanceTo || distanceTo == 0) &&
               (currentTimeBuff == -15 && s.Sprint.Interval == 15 ? s.Sprint.StartDateTime > now : s.Sprint.StartDateTime > DateTime.UtcNow.AddMinutes(-(s.Sprint.Interval)));


            var sprintsCommon = this.SprintParticipantRepo.GetAll(creatorQueryCommon).ToList();
            var friendsRelationsCommon = this.SprintParticipantRepo.GetFriends(userId);
            var friendsCommon = friendsRelationsCommon.Select(f => f.AcceptedUserId == userId ? f.SharedUserId : f.AcceptedUserId);

            var joinedSprintDtoCommon = new GetCommonSprintDto();

            var creatorEventCommon = this.SprintParticipantRepo.GetAll(creatorQueryCommon);
            Expression<Func<SprintParticipant, bool>> pqueryCommon = s =>
                   s.User.Id != userId &&
                   (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
                   s.User.UserState == UserState.Active;

            var otherCommon = sprintsCommon.Select(s => new GetCommonSprintDto()
            {
                SprintInfo = new SprintInfoDTO()
                {
                    Id = s.Sprint.Id,
                    Name = s.Sprint.Name,
                    Distance = s.Sprint.Distance,
                    StartTime = s.Sprint.StartDateTime,
                    ExtendedTime = s.Sprint.StartDateTime.AddMinutes(s.Sprint.Interval),
                    Type = s.Sprint.Type,
                    Creator = s.Sprint.CreatedBy.Id == s.UserId,
                    NumberOfParticipants = s.Sprint.NumberOfParticipants,
                    ImageUrl = s.Sprint.ImageUrl,
                    PromoCode = s.Sprint.PromotionCode,
                    TimebasedDescription = s.Sprint.DescriptionForTimeBasedEvent,
                    IsTimebased = s.Sprint.IsTimeBased,
                    DurationForTimeBasedEvent = s.Sprint.DurationForTimeBasedEvent

                },
                ParticipantInfo = this.SprintParticipantRepo.GetAllById(s.Sprint.Id, pqueryCommon).Select(
                 sp => new ParticipantInfoDTO()
                 {
                     Id = sp.User.Id,
                     Name = sp.User.Name,
                     ColorCode = sp.User.ColorCode,
                     IsFriend = friendsCommon.Contains(sp.User.Id)
                 }
             ).ToList()
            }); ;

            return otherCommon.ToList();
            //if (creatorEventCommon != null)
            //{
            //    joinedSprintDtoCommon.SprintInfo = creatorEventCommon.Select(
            //      sp => new SprintInfoDTO()
            //      {
            //          Id = sp.Sprint.Id,
            //          Name = sp.Sprint.Name,
            //          Distance = sp.Sprint.Distance,
            //          StartTime = sp.Sprint.StartDateTime,
            //          ExtendedTime = sp.Sprint.StartDateTime.AddMinutes(15),
            //          Type = sp.Sprint.Type,
            //          Creator = sp.Sprint.CreatedBy.Id == sp.UserId,
            //          NumberOfParticipants = s.Sprint.NumberOfParticipants
            //      }
            //  ).ToList();


            //    var sprints = this.SprintParticipantRepo.GetAll(query).ToList();
            //var friendsRelations = this.SprintParticipantRepo.GetFriends(userId);
            //var friends = friendsRelations.Select(f => f.AcceptedUserId == userId ? f.SharedUserId : f.AcceptedUserId);
            //var joinedSprintDto = new GetSprintDto();

            //Expression<Func<SprintParticipant, bool>> pquery = s =>
            //        s.User.Id != userId &&
            //        (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
            //        s.User.UserState == UserState.Active;


            //var other = sprints.Select(s => new JoinedSprintDTO()
            //{
            //    SprintInfo = new SprintInfoDTO()
            //    {
            //        Id = s.Sprint.Id,
            //        Name = s.Sprint.Name,
            //        Distance = s.Sprint.Distance,
            //        StartTime = s.Sprint.StartDateTime,
            //        ExtendedTime = s.Sprint.StartDateTime.AddMinutes(15),
            //        Type = s.Sprint.Type,
            //        NumberOfParticipants = s.Sprint.NumberOfParticipants
            //    },
            //    ParticipantInfo = this.SprintParticipantRepo.GetAllById(s.Sprint.Id, pquery).Select(
            //      sp => new ParticipantInfoDTO()
            //      {
            //          Id = sp.User.Id,
            //          Name = sp.User.Name,
            //          ColorCode = sp.User.ColorCode,
            //          IsFriend = friends.Contains(sp.User.Id)

            //      }
            //  ).ToList()
            //});

            //joinedSprintDto.Other = other.ToList();

            //Expression<Func<SprintParticipant, bool>> creatorQuery = s =>
            //    s.UserId == userId &&
            //    s.User.UserState == UserState.Active &&
            //    (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
            //    s.Sprint.CreatedBy.Id == userId &&
            //    s.Sprint.Status != (int)SprintStatus.ARCHIVED &&
            //    (s.Sprint.StartDateTime > now);

            //var creatorEvent = this.SprintParticipantRepo.GetAll(creatorQuery);
            //if (creatorEvent != null)
            //{
            //    joinedSprintDto.Creator = creatorEvent.Select(
            //      sp => new SprintInfoDTO()
            //      {
            //          Id = sp.Sprint.Id,
            //          Name = sp.Sprint.Name,
            //          Distance = sp.Sprint.Distance,
            //          StartTime = sp.Sprint.StartDateTime,
            //          ExtendedTime = sp.Sprint.StartDateTime.AddMinutes(15),
            //          Type = sp.Sprint.Type,
            //          Creator = sp.Sprint.CreatedBy.Id == sp.UserId
            //      }
            //  ).ToList();

            //var creatorDto = new SprintInfoDTO()
            //{
            //    Id = creatorEvent.Sprint.Id,
            //    Name = creatorEvent.Sprint.Name,
            //    Distance = creatorEvent.Sprint.Distance,
            //    StartTime = creatorEvent.Sprint.StartDateTime,
            //    ExtendedTime = creatorEvent.Sprint.StartDateTime.AddMinutes(15),
            //    Type = creatorEvent.Sprint.Type,
            //    Creator = creatorEvent.Sprint.CreatedBy.Id == creatorEvent.UserId
            //};
            //joinedSprintDto.Creator = creatorDto;
            //}
            //return otherCommon;
        }

        /// <summary>
        /// Get sprint details with who marked attendance with given user id
        /// </summary>
        /// <param name="userId">user id to get record</param>
        /// <returns><see cref=" SprintInfo">class </see></returns>
        public async Task<SprintInfo> GetSprintWhichMarkedAttendance(int userId)
        {
            var expiredDate = DateTime.UtcNow.AddHours(-8);
            Expression<Func<SprintParticipant, bool>> query = s =>
                s.UserId == userId &&
                s.User.UserState == UserState.Active &&
                s.Sprint.StartDateTime > expiredDate &&
                s.Stage == ParticipantStage.MARKED_ATTENDENCE;
            var markedAttendaceDetails = await this.SprintParticipantRepo.Get(query);
            if (markedAttendaceDetails != null)
            {
                string strCoHost = string.Empty;
                if (markedAttendaceDetails.Sprint.InfluencerEmailSecond != null && markedAttendaceDetails.Sprint.InfluencerEmailSecond.Trim() != string.Empty && StringUtils.IsBase64String(markedAttendaceDetails.Sprint.InfluencerEmailSecond))
                    strCoHost = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(markedAttendaceDetails.Sprint.InfluencerEmailSecond);

                return new SprintInfo(
                    markedAttendaceDetails.Sprint.Id,
                    markedAttendaceDetails.Sprint.Name,
                    markedAttendaceDetails.Sprint.Distance,
                    markedAttendaceDetails.Sprint.StartDateTime,
                    markedAttendaceDetails.Sprint.Type,
                    markedAttendaceDetails.IsIinfluencerEventParticipant,
                    false,
                    markedAttendaceDetails.Sprint.IsTimeBased,
                    markedAttendaceDetails.Sprint.DurationForTimeBasedEvent,
                    markedAttendaceDetails.Sprint.DescriptionForTimeBasedEvent,
                    markedAttendaceDetails.Sprint.IsNarrationsOn ,
                    strCoHost
                    );

            }
            else
            {
                throw new Application.ApplicationException("NOT_FOUND_MARKED_ATTENDACE ");
            }
        }

        /// <summary>
        /// Invite user to sprint
        /// </summary>
        /// <param name="                sprintId ">sprint id</param>
        /// <param name="                inviterId ">id of inviter</param>
        /// <param name="                inviteeIds ">ids for invitess</param>
        /// <returns>invited users info</returns>
        public async Task<List<ParticipantInfoDto>> SprintInvite(int sprintId, int inviterId, List<int> inviteeIds)
        {
            var sprint = await this.SprintParticipantRepo.GetSprint(sprintId);
            if (sprint == null)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.SprintNotFound, "Sprint not found ");
            }
            var participantInfoDtos = new List<ParticipantInfoDto>();
            foreach (int inviteeId in inviteeIds)
            {
                var user = await this.SprintParticipantRepo.CheckSprintParticipant(sprintId, inviteeId);
                if (user == null)
                {
                    var invitee = await this.SprintParticipantRepo.GetParticipant(inviteeId);
                    if (invitee == null)
                    {
                        throw new Application.SCApplicationException((int)ErrorCodes.NotAllowedOperation, $"Invitee not found, inviteeId = { inviteeId }");
                    }
                    await this.SprintParticipantRepo.AddParticipant(sprintId, inviteeId);
                    participantInfoDtos.Add(new ParticipantInfoDto(
                        invitee.Id,
                        invitee.Name,
                        invitee.ProfilePicture,
                        invitee.Code,
                        invitee.ColorCode,
                        invitee.City,
                        invitee.Country,
                        invitee.CountryCode,
                        ParticipantStage.PENDING
                    ));
                }
                else
                {
                    if (user.Stage == ParticipantStage.QUIT)
                    {
                        user.Stage = ParticipantStage.PENDING;
                        participantInfoDtos.Add(new ParticipantInfoDto(
                            user.User.Id,
                            user.User.Name,
                            user.User.ProfilePicture,
                            user.User.Code,
                            user.User.ColorCode,
                            user.User.City,
                            user.User.Country,
                            user.User.CountryCode,
                            ParticipantStage.PENDING
                        ));
                    }
                }
            };
            this.SprintParticipantRepo.SaveChanges();
            foreach (ParticipantInfoDto pariticipantInfo in participantInfoDtos)
            {
                this.NotificationClient.SprintNotificationJobs.SprintInvite(sprintId, inviterId, pariticipantInfo.Id);
            }
            return participantInfoDtos;
        }

        /// <summary>
        /// Get all notificaitons
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>all notificaiton related to given userid</returns>
        public Notifications GetNotification(int userId)
        {

            var notifications = this.SprintParticipantRepo.GetNotification(userId);
            //var result = new List<object>();

            //var resultNew = new List<object>();
            //var resultToday = new List<object>();
            //var resultOlder = new List<object>();

            Notifications notification = new Notifications();

            notifications
                .OrderByDescending(n => n.Notification.CreatedDate)
                .ToList().ForEach(s =>
                {
                    if (s.BadgeCount == 1)
                    {
                        // New Notifications
                        switch (s.Notification)
                        {
                            case SprintNotification sprintTypeNotification:
                                notification.ResultNew.Add(NotificationDtoFactory.Build(s.Sender, s.Receiver, sprintTypeNotification));
                                break;
                            case AchievementNoticiation achievementTypeNotification:
                                notification.ResultNew.Add(NotificationDtoFactory.AchievementBuild(achievementTypeNotification));
                                break;
                            default:
                                break;
                        }

                        //notification.Result.Add(notification.ResultNew);

                    }
                    else if (s.BadgeCount != 1 && s.CreatedDate.Date == DateTime.UtcNow.Date)
                    {
                        //Today Notification
                        switch (s.Notification)
                        {
                            case SprintNotification sprintTypeNotification:
                                notification.ResultToday.Add(NotificationDtoFactory.Build(s.Sender, s.Receiver, sprintTypeNotification));
                                break;
                            case AchievementNoticiation achievementTypeNotification:
                                notification.ResultToday.Add(NotificationDtoFactory.AchievementBuild(achievementTypeNotification));
                                break;
                            default:
                                break;
                        }

                        // notification.Result.Add(notification.ResultToday);
                    }
                    else
                    {
                        //Older Notification
                        switch (s.Notification)
                        {
                            case SprintNotification sprintTypeNotification:
                                notification.ResultOlder.Add(NotificationDtoFactory.Build(s.Sender, s.Receiver, sprintTypeNotification));
                                break;
                            case AchievementNoticiation achievementTypeNotification:
                                notification.ResultOlder.Add(NotificationDtoFactory.AchievementBuild(achievementTypeNotification));
                                break;
                            default:
                                break;
                        }

                        // notification.Result.Add(notification.ResultOlder);
                    }

                });
            // set badge cout to "0" for the requested user
            this.SprintParticipantRepo.UpdateBadgeCountByUserId(userId);
            return notification;
        }

        /// <summary>
        /// Remove sprint participant form  sprint
        /// </summary>
        /// <param name="requesterId">requester user id</param>
        /// <param name="sprintId">sprint id</param>
        /// <param name="participantId">participant id for remove</param>
        public async Task RemoveParticipant(int requesterId, int sprintId, int participantId)
        {
            Expression<Func<SprintParticipant, bool>> query = s => s.SprintId == sprintId && s.UserId == participantId && s.User.UserState == UserState.Active;
            var sprintParticipant = await this.SprintParticipantRepo.Get(query);
            if (sprintParticipant == null)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.ParticipantNotFound, "Participant not found ");
            }
            if (sprintParticipant.Sprint.CreatedBy.Id != requesterId)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.CanNotRemoveParticipant, "Creator only remove participant ");
            }
            else if (sprintParticipant.Sprint.Type != (int)SprintType.PrivateSprint)
            {
                throw new Application.SCApplicationException((int)ErrorCodes.NotAllowedOperation, "Can only remove participant from private event ");
            }
            // else if (sprintParticipant.Sprint.StartDateTime.AddMinutes(-10) < DateTime.UtcNow)
            // {
            //     throw new Application.SCApplicationException((int)ErrorCodes.MarkAttendanceEnable, "Mark Attendance enable.can 't remove pariticiapnt");
            // }

            this.SprintParticipantRepo.RemoveParticipant(sprintParticipant);
            // this.SprintParticipantRepo.RemoveSprintNotification(sprintId, participantId);
            this.SprintParticipantRepo.SaveChanges();
            this.NotificationClient.SprintNotificationJobs.SprintParticipantRemove(
                sprintParticipant.SprintId,
                (SprintType)sprintParticipant.Sprint.Type,
                (SprintStatus)sprintParticipant.Sprint.Status,
                sprintParticipant.Sprint.CreatedBy.Id,
                sprintParticipant.User.Id,
                sprintParticipant.Sprint.CreatedBy.Name,
                sprintParticipant.Sprint.Name,
                sprintParticipant.Sprint.StartDateTime,
                sprintParticipant.Sprint.NumberOfParticipants,
                sprintParticipant.Sprint.Distance,
                sprintParticipant.Sprint.Name,
                sprintParticipant.User.ProfilePicture,
                sprintParticipant.User.Code,
                sprintParticipant.User.Country,
                sprintParticipant.User.CountryCode,
                sprintParticipant.User.City);

        }

        /// <summary>
        /// Get friend status in sprint
        /// </summary>
        /// <param name="userId">user id </param>
        /// <param name="sprintId">sprint id</param>
        /// <returns><see cref="FriendInSprintDto">friend in sprint </see></returns>
        public List<FriendInSprintDto> GetFriendsStatusInSprint(int userId, int sprintId)
        {
            var friendsRelations = this.SprintParticipantRepo.GetFriends(userId);
            var friends = friendsRelations.Select(f => f.AcceptedUserId == userId ? f.SharedUser : f.AcceptedUser);
            Expression<Func<SprintParticipant, bool>> query = s =>
                s.SprintId == sprintId &&
                s.UserId != userId &&
                s.Stage != ParticipantStage.QUIT &&
                s.User.UserState == UserState.Active;
            var sprintParticipantsIds = this.SprintParticipantRepo.GetAll(query).Select(s => s.UserId).ToList();
            var result = friends.Select(f => new FriendInSprintDto(
                    f.Id,
                    f.Name,
                    f.ProfilePicture,
                    f.City,
                    f.Country,
                    f.CountryCode,
                    f.ColorCode,
                    sprintParticipantsIds.Contains(f.Id)
                ))
                .ToList();
            return result;
        }

        /// <summary>
        /// Remove notification
        /// </summary>
        /// <param name="notificationId">notificaiton id to remove</param>
        public async Task RemoveNotification(int notificationId)
        {
            await this.SprintParticipantRepo.RemoveNotification(notificationId);
            this.SprintParticipantRepo.SaveChanges();
            return;
        }

        /// <summary>
        /// Get statistics for given user id
        /// </summary>
        /// <param name="userId"> user id to fetch</param>
        /// <returns>get all statistics for public and private sprints </returns>
        public SprintStatisticDto GetStatistic(int userId)
        {
            Expression<Func<SprintParticipant, bool>> query = s =>
                s.UserId == userId &&
                (s.Stage == ParticipantStage.COMPLETED) &&
                s.User.UserState == UserState.Active;
            var allCompletedEvents = this.SprintParticipantRepo.GetAll(query).ToList();
            var statistics = new SprintStatisticDto();
            allCompletedEvents.ForEach(s =>
            {
                if (s.Sprint.Type == (int)SprintType.PublicSprint)
                {
                    statistics.SetPublicEvent(s);
                }
                else
                {
                    statistics.SetPrivateEvent(s);
                }
            });
            return statistics;
        }

        /// <summary>
        /// Get Participant Sprints History
        /// </summary>
        /// <param name = "userId" ></ param >
        /// < returns ></ returns >
        public Task<List<Sprint>> GetAllSprintsHistoryByUserId(int userId, int pageNo, int limit)
        {

            return this.SprintParticipantRepo.GetAllSprintsHistoryByUserId(userId, pageNo, limit);

        }

        /// <summary>
        /// Get All Sprints History Count By UserId
        /// </summary>
        /// <param name = "userId" ></ param >
        /// < returns ></ returns >
        public Task<int> GetAllSprintsHistoryCountByUserId(int userId)
        {

            return this.SprintParticipantRepo.GetAllSprintsHistoryCountByUserId(userId);
        }

        /// <summary>
        /// Get all joined sprints for given date
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <param name="fetchDate">fetch date</param>
        /// <returns>joined sprints</returns>
        public JoinedSprintsDto GetJoinedEvents(int userId, DateTime fetchDate)
        {
            var sprints = this.SprintParticipantRepo.GetJoinedSprints(userId, fetchDate);
            var joinSprintDto = new List<JoinedSprintDto>();
            foreach (var sprint in sprints)
            {
                var totalCount = this.SprintParticipantRepo.GetParticipantCount(sprint.Id);
                joinSprintDto.Add(new JoinedSprintDto(sprint, totalCount));
            }
            var dates = this.SprintParticipantRepo.GetNextSevenDaysSprintsDates(userId);

            return new JoinedSprintsDto(joinSprintDto, dates);
        }

        /// <summary>
        /// Update sprint completed or not and time
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sprintId"></param>
        /// <param name="time"></param>
        /// <param name="stage"></param>
        public async Task UpdateParticipantStatus(int userId, int sprintId, DateTime time, ParticipantStage stage, double position, string raceCompletedDuration, double distance)
        {
            Expression<Func<SprintParticipant, bool>> query = s => s.UserId == userId && s.SprintId == sprintId;
            var participant = await this.SprintParticipantRepo.Get(query);
            participant.Stage = stage;
           
            if (participant.Sprint.IsTimeBased)
            {
                participant.DistanceRan = (int)distance;
                participant.RaceCompletedDuration = DateTime.UtcNow.Subtract(participant.StartedTime).ToString();
            }
            else
            {
                participant.DistanceRan = participant.Sprint.Distance;
            }

            participant.FinishTime = time;
            if (position != 0)
                participant.Position = position;
           

            try
            {

                if (stage == ParticipantStage.COMPLETED)
                {
                    GpsLogApiConsumer gpsApi = new GpsLogApiConsumer();
                    int totalElevation = await gpsApi.GetTotalElevation(sprintId, userId);
                    Log.Logger.Information($" totalElevation - {totalElevation}");
                    participant.TotalElevation = totalElevation;
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Information($" GetTotalElevation - {ex}");
            }

            this.SprintParticipantRepo.UpdateParticipant(participant);
            this.SprintParticipantRepo.SaveChanges();
            return;
        }

        public async Task SprintExpired(int sprintId, List<NotCompletedRunners> notCompletedRunners)
        {
            var sprint = await this.SprintParticipantRepo.GetSprint(sprintId);
            sprint.Status = (int)SprintStatus.ENDED;
            Expression<Func<SprintParticipant, bool>> query = s => s.SprintId == sprintId;
            var participants = await this.SprintParticipantRepo.GetAllParticipants(query);

            foreach (SprintParticipant participant in participants)
            {
                // var participant = participants.Current;
                if (participant.Stage != ParticipantStage.COMPLETED)
                {
                    var runner = notCompletedRunners.FirstOrDefault(n => n.UserId == participant.UserId);
                    participant.Stage = ParticipantStage.QUIT;
                    if (runner != null)
                    {
                        participant.DistanceRan = (int)runner.DistanceRun;
                    }
                    participant.FinishTime = sprint.StartDateTime.AddHours(8);
                    this.SprintParticipantRepo.UpdateParticipant(participant);
                }
            }
            this.SprintParticipantRepo.SaveChanges();

            //while (participants.MoveNext())
            //{
            //    var participant = participants.Current;
            //    if (participant.Stage != ParticipantStage.COMPLETED)
            //    {
            //        var runner = notCompletedRunners.FirstOrDefault(n => n.UserId == participant.UserId);
            //        participant.Stage = ParticipantStage.QUIT;
            //        if (runner != null)
            //        {
            //            participant.DistanceRan = (int)runner.DistanceRun;
            //        }
            //        participant.FinishTime = sprint.StartDateTime.AddHours(20);
            //        this.SprintParticipantRepo.UpdateParticipant(participant);
            //    }

            //}

        }

        public async Task<SprintParticipantDto> GetSprintParticipant(int sprintId,int userId)
        {
            var user = await this.SprintParticipantRepo.CheckSprintParticipant(sprintId, 2953);
            return new SprintParticipantDto()
            {
                DistanceRan = user.DistanceRan,
                FinishTime = user.FinishTime,
                StartedTime = user.StartedTime
            };

        }

        
        public async Task<SprintInfo> GetSprint(int sprintId)
        {
            var sprint = await this.SprintParticipantRepo.GetSprint(sprintId);
            return new SprintInfo(sprint);
        }
    }

    public class Notifications //<T> where T : class, new()
    {
        public List<object> ResultNew { get; set; }
        public List<object> ResultToday { get; set; }
        public List<object> ResultOlder { get; set; }

        //public List<object> Result { get; set; }

        public Notifications()
        {
            this.ResultNew = new List<object>();
            this.ResultToday = new List<object>();
            this.ResultOlder = new List<object>();
            // this.Result = new List<object>();
        }
    }
}