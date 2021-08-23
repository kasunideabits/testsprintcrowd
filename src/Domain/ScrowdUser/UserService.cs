namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Domain.Achievement;
    using SprintCrowd.BackEnd.Domain.Friend;
    using System.Collections.Generic;
    using SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Account;
    using SprintCrowd.BackEnd.Web.PushNotification;
    using SprintCrowd.BackEnd.Web.ScrowdUser.Models;
    using SprintCrowd.BackEnd.Utils;
    using SprintCrowdBackEnd.Domain.ScrowdUser.Dtos;
    using System.Linq.Expressions;
    using System;
    using System.Linq;
    using SprintCrowd.BackEnd.Enums;


    /// <summary>
    /// user service used for managing users.
    /// Authorization dosent happen here.
    /// Auth happens in identity server.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepo">instance of userRepo, dependency injected.</param>

        public UserService(IUserRepo userRepo, ISprintParticipantService _sprintParticipantService, IFriendService frinedService, ISprintParticipantService sprintParticipantService, IAchievementService serviceAchievement)
        {
            this.userRepo = userRepo;
            this.sprintParticipantService = _sprintParticipantService;
            this.FriendService = frinedService;
            this.SprintParticipantService = sprintParticipantService;
            this.AchievementService = serviceAchievement;
        }

        private IFriendService FriendService { get; }


        private ISprintParticipantService sprintParticipantService { get; }

        private ISprintParticipantService SprintParticipantService { get; }

        private IAchievementService AchievementService { get; }

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <param name="userId">user id for lookup</param>
        /// <returns><see cref="UserDto"> user info details </see></returns>
        public async Task<UserDto> GetUser(int? userId)
        {
            if (userId != null)
            {
                var user = await this.userRepo.GetUser((int)userId);
                var userRoles = await this.GetUserRoleInfo((int)userId);
                return new UserDto(user, userRoles);
            }
            else
            {
                throw new Application.ApplicationException("User Not Found");
            }
        }



        /// <summary>
        /// Get facebook User.
        /// </summary>
        /// <param name="userId">Facebook user id.</param>
        public async Task<User> GetFacebookUser(string userId)
        {
            var result = await this.userRepo.GetFacebookUser(userId);
            if (result.UserState != Application.UserState.Active)
            {
                result.UserState = Application.UserState.Active;
                this.userRepo.UpdateUser(result);
                this.userRepo.SaveChanges();
            }
            return result;
        }


        /// <summary>
        /// get influncer details from email
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserDto> getUserByEmail(string email)
        {
            User influncerUser = null;
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
            influncerUser = await this.userRepo.findUserByEmail(encryptedEamil);
            if (influncerUser == null)
            {
                influncerUser = await this.userRepo.findUserByEmail(email);
            }
            return new UserDto(influncerUser != null ? influncerUser : new User());
        }

        /// <summary>
        /// register a user
        /// if multiple user types are needed, facebook, google etc
        /// then heres the place to make the change.
        /// </summary>
        /// <param name="registerData">registeration data.</param>
        public async Task<User> RegisterUser(RegisterModel registerData)
        {
            User user = await this.userRepo.RegisterUser(registerData);
            await this.userRepo.AddUserPreference(user.Id);
            await this.userRepo.AddDefaultUserSettings(user.Id);
            await this.userRepo.AddUserRole(user.Id, Enums.UserRoles.User);
            this.userRepo.SaveChanges();
            return user;
        }

        /// <summary>
        /// user search
        /// </summary>
        /// <param name="searchParams">registeration data.</param>
        public async Task<List<UserSelectDto>> UserSearch(string searchParams)
        {
            List<User> users = await this.userRepo.GetUsersBySearch(searchParams);
            return users.ConvertAll(user => new UserSelectDto()
            {
                Name = user.Name,
                Image = user.ProfilePicture,
                Email = user.Email,
            });

        }

        /// <summary>
        /// register a email user
        /// if multiple user types are needed
        /// then heres the place to make the change.
        /// </summary>
        /// <param name="registerData">registeration data.</param>
        public async Task<User> RegisterEmailUser(EmailUser registerData)
        {
            User user = await this.userRepo.RegisterEmailUser(registerData);
            await this.userRepo.AddUserPreference(user.Id);
            await this.userRepo.AddDefaultUserSettings(user.Id);
            await this.userRepo.AddUserRole(user.Id, Enums.UserRoles.User);
            // await this.userRepo.AddPromocodeUser(user.Id, registerData.PromotionCode, registerData.SprintId);
            this.userRepo.SaveChanges();
            //Promocode User join to the sprint

            return user;
        }

        /// <summary>
        /// Add Promotion Code
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<Sprint> AddPromotionCode(EmailUser registerData, int userId)
        {
            //Get the sprint Id related to promotion code.
            Sprint sprint = await this.userRepo.GetSprintByPromoCode(registerData.PromotionCode);

            if (sprint != null)
            {
                ///Add Promotion code user details.
                await this.userRepo.AddPromocodeUser(userId, registerData.PromotionCode, sprint.Id);
                this.userRepo.SaveChanges();
                //join to sprint after adding promotion code
                await this.sprintParticipantService.JoinSprint(
                  sprint.Id,
                  userId,
                  0,
                  true
              );
            }
            else
            {
                throw new Application.SCApplicationException((int)ErrorCodes.InvalidPromotionCode, "Promotion code is invalid or expired.");
            }

            return sprint;
        }

        /// <summary>
        /// Email Confirmation By Mail
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> EmailConfirmationByMail(EmailUser registerData)
        {
            bool success = await this.userRepo.EmailConfirmationByMail(registerData);
            //await this.userRepo.AddUserPreference(user.Id);
            //await this.userRepo.AddDefaultUserSettings(user.Id);
            //this.userRepo.SaveChanges();
            return success;
        }

        /// <summary>
        /// Generate Email User Token For Password Reset
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> GenerateEmailUserTokenForPwReset(EmailUser registerData)
        {
            bool success = await this.userRepo.GenerateEmailUserTokenForPwReset(registerData);
            return success;
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> ResetPassword(EmailUser registerData)
        {
            bool success = await this.userRepo.ResetPassword(registerData);
            return success;
        }

        /// <summary>
        /// saves fcm token
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="fcmToken">fcm token</param>
        /// <returns>async task</returns>
        public async Task SaveFcmToken(int userId, string fcmToken)
        {
            await this.userRepo.SaveFcmToken(userId, fcmToken);
            this.userRepo.SaveChanges();
        }

        /// <summary>
        /// Save user activity
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>async task</returns>
        public async Task<UserActivity> UpdateUserActivity(int userId)
        {
            UserActivity userActivity = await this.userRepo.UpdateUserActivity(userId);
            this.userRepo.SaveChanges();
            return userActivity;
        }

        /// <summary>
        /// Get user preference
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>user preference</returns>
        public async Task<UserPreferenceDto> GetUserPreference(int userId)
        {
            var userPreference = await this.userRepo.GetUserPreference(userId);
            if (userPreference == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User Preference not found");
            }

            return new UserPreferenceDto(userPreference);
        }

        /// <summary>
        /// Update user preferences
        /// </summary>
        /// <param name="userId"> user id to for update user </param>
        /// <param name="userPreferenceModel">user preference</param>
        /// <returns>updated user preference</returns>
        public async Task<UserPreferenceDto> UpdateUserPreference(int userId, UserPreferenceModel userPreferenceModel)
        {
            var userPreference = await this.userRepo.GetUserPreference(userId);
            if (userPreference == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User Preference not found");
            }
            userPreference.Mon = userPreferenceModel.Day.Mon;
            userPreference.Tue = userPreferenceModel.Day.Tue;
            userPreference.Wed = userPreferenceModel.Day.Wed;
            userPreference.Thur = userPreferenceModel.Day.Thur;
            userPreference.Fri = userPreferenceModel.Day.Fri;
            userPreference.Sat = userPreferenceModel.Day.Sat;
            userPreference.Sun = userPreferenceModel.Day.Sun;
            userPreference.Morning = userPreferenceModel.Time.Morning;
            userPreference.AfterNoon = userPreferenceModel.Time.AfterNoon;
            userPreference.Evening = userPreferenceModel.Time.Evening;
            userPreference.Night = userPreferenceModel.Time.Night;
            userPreference.TwoToFive = userPreferenceModel.Distance.TwoToFive;
            userPreference.SixToTen = userPreferenceModel.Distance.SixToTen;
            userPreference.ElevenToFifteen = userPreferenceModel.Distance.ElevenToFifteen;
            userPreference.SixteenToTwenty = userPreferenceModel.Distance.SixteenToTwenty;
            userPreference.TOneToThirty = userPreferenceModel.Distance.TOneToThirty;
            userPreference.ThirtyOneToFortyOne = userPreferenceModel.Distance.ThirtyOneToFortyOne;
            this.userRepo.UpdateUserPreference(userPreference);
            this.userRepo.SaveChanges();
            return new UserPreferenceDto(userPreference);
        }

        /// <summary>
        /// Get user settings with given user id
        /// </summary>
        /// <param name="userId">user id to fetch settings</param>
        /// <returns>user settings</returns>
        public async Task<UserSettingsDto> GetUserSettings(int userId)
        {
            var userSettings = await this.userRepo.GetUserSettings(userId);
            if (userSettings == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User settings not found");
            }
            return new UserSettingsDto(userSettings.User.LanguagePreference, userSettings);
        }

        /// <summary>
        /// Update user settings
        /// </summary>
        /// <param name="userId"> user id to for update user </param>
        /// <param name="userSettingsModel">user settings</param>
        /// <returns>updated user settings</returns>
        public async Task<UserSettingsDto> UpdateUserSettings(int userId, UserSettingsModel userSettingsModel)
        {
            var userSettings = await this.userRepo.GetUserSettings(userId);
            if (userSettings == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User settings not found");
            }
            userSettings.TwentyFourH = userSettingsModel.Reminder.TwentyForH;
            userSettings.OneH = userSettingsModel.Reminder.OneH;
            userSettings.FiftyM = userSettingsModel.Reminder.FiftyM;
            userSettings.EventStart = userSettingsModel.Reminder.EventStart;
            userSettings.FinalCall = userSettingsModel.Reminder.FinalCall;
            this.userRepo.UpdateUserSettings(userSettings);
            var user = await this.userRepo.GetUser(userId);
            if (user.LanguagePreference != userSettingsModel.Language)
            {
                user.LanguagePreference = userSettingsModel.Language;
                this.userRepo.UpdateUser(user);
            }
            this.userRepo.SaveChanges();
            return new UserSettingsDto(user.LanguagePreference, userSettings);
        }

        /// <summary>
        /// Deactivate account
        /// </summary>
        /// <param name="userId">user id to deactivate</param>
        public async Task AccountDeactivate(int userId)
        {
            var user = await this.userRepo.GetUser(userId);
            user.UserState = Application.UserState.Deactivate;
            this.userRepo.UpdateUser(user);
            this.userRepo.SaveChanges();
        }

        /// <summary>
        /// Logout user account
        /// </summary>
        /// <param name="userId">user id to logout</param>
        public async Task AccountLogout(int userId)
        {
            var user = await this.userRepo.GetUser(userId);
            user.UserState = Application.UserState.Logout;
            this.userRepo.UpdateUser(user);
            this.userRepo.SaveChanges();
        }

        /// <summary>
        /// Is User Exist In SC
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserExistDto> IsUserExistInSC(string email)
        {
            return await this.userRepo.IsUserExistInSC(email);
        }



        /// <summary>
        /// View User Profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserProfileDto> ViewUserProfile(int userId, int loggedUserId)
        {
            //Get user profile detail
            var userInfor = await this.GetUser(userId);
            //Get user related friends
            var allFriends = await this.FriendService.AllFriends(userId);
            //logged users friends
            var myFriends = await this.FriendService.AllFriends(loggedUserId);

            //check users freind is friend of mine
            foreach(var friend in allFriends)
            {
                if(myFriends.Where(x=>x.Id == friend.Id).Any())
                {
                    friend.IsFreindOfMine = true;
                }                 
            }           

            //Get user sprint statstics
            var userStatistic = this.SprintParticipantService.GetStatistic(userId);
            //Get user achievement
            var userAchievement = this.AchievementService.Get(userId);

            UserProfileDto userProfileDetail = new UserProfileDto(
                userInfor.UserId,
                userInfor.Name,
                userInfor.Description,
                userInfor.ProfilePicture,
                userInfor.CountryCode,
                userInfor.JoinedDate,
                allFriends,
                userStatistic,
                userAchievement,
                userInfor.UserShareType);

            return userProfileDetail;
        }


        /// <summary>
        /// View User Profile with sprint details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserProfileDto> ViewUserProfileWithSprints(int userId, int loggedUserId)
        {
            int inviteId = 0;
            //Get user profile detail
            var userInfor = await this.GetUser(userId);
            //Get user related friends
            var allFriends = await this.FriendService.AllFriends(userId);
            //logged users friends
            var myFriends = await this.FriendService.AllFriends(loggedUserId);

            var invites = await this.FriendService.InvitationsListSentByUser(loggedUserId);

            var invite = invites.Where(x => x.ToUserId == userId).FirstOrDefault();

            bool isMyFriend = false;

            if (myFriends != null)
            {
                isMyFriend = myFriends.Where(x => x.Id == userId).Any();
            }

            if (invite != null)
            {
                inviteId = invite.Id;
            }

            //check users freind is friend of mine
            foreach (var friend in allFriends)
            {
                if (myFriends.Where(x => x.Id == friend.Id).Any())
                {
                    friend.IsFreindOfMine = true;
                }
            }

            //Get user sprint statstics
            var sprintData = this.SprintParticipantService.GetSprintWithParticipantProfile(userId);
            //Get user achievement
            var userAchievement = this.AchievementService.Get(userId);

            UserProfileDto userProfileDetail = new UserProfileDto(
                userInfor.UserId,
                userInfor.Name,
                userInfor.Description,
                userInfor.ProfilePicture,
                userInfor.CountryCode,
                userInfor.JoinedDate,
                allFriends,
                null,
                userAchievement,
                userInfor.UserShareType,
                sprintData,
                inviteId,
                isMyFriend);

            return userProfileDetail;
        }

        /// <summary>
        /// View User Profile
        /// </summary>
        /// <param name="userId"></param>

        /// <returns></returns>
        public async Task<UserProfileDto> ViewUserProfile(int userId)
        {
            //Get user profile detail
            var userInfor = await this.GetUser(userId);
            //Get user related friends
            var allFriends = await this.FriendService.AllFriends(userId);
            //Get user sprint statstics
            var userStatistic = this.SprintParticipantService.GetStatistic(userId);
            //Get user achievement
            var userAchievement = this.AchievementService.Get(userId);

            UserProfileDto userProfileDetail = new UserProfileDto(
                userInfor.UserId,
                userInfor.Name,
                userInfor.Description,
                userInfor.ProfilePicture,
                userInfor.CountryCode,
                userInfor.JoinedDate,
                allFriends,
                userStatistic,
                userAchievement,
                userInfor.UserShareType);

            return userProfileDetail;
        }

        /// <summary>
        /// Get User App Version Upgrade Info
        /// </summary>
        /// <param name="userOS"></param>
        /// <param name="userCurrentAppVersion"></param>
        /// <returns></returns>
        public async Task<UserAppVersionInfo> GetUserAppVersionUpgradeInfo(string userOS, string userCurrentAppVersion)
        {
            try
            {
                return await this.userRepo.GetUserAppVersionUpgradeInfo(userOS, userCurrentAppVersion);
            }
            catch (System.Exception Ex)
            {
                throw Ex;
            }
        }

        public async Task<UserProfileDto> UpdateUserProfile(UserProfileDto updateUserProfile)
        {
            try
            {
                var user = await this.userRepo.GetUser(updateUserProfile.UserId);
                user.UserShareType = updateUserProfile.UserShareType;
                user.Country = updateUserProfile.Country;
                user.Name = updateUserProfile.Name;
                user.Description = updateUserProfile.Description;
                user.ProfilePicture = updateUserProfile.ProfilePicture;
                var result = this.userRepo.UpdateUserAndReturn(user);

                UserProfileDto userpofileDto = new UserProfileDto()
                {
                    UserId = result.Id,
                    Name = result.Name,
                    ProfilePicture = result.ProfilePicture,
                    Description = result.Description,
                    CountryCode = result.CountryCode,
                    UserShareType = result.UserShareType
                };

                return userpofileDto;
            }
            catch (System.Exception Ex)
            {
                throw Ex;
            }
        }


        public async Task<bool> DeleteUserProfile(int UserId)
        {
            try
            {
                var user = await this.userRepo.GetUser(UserId);
                
                if(user == null)
                {
                    return false;
                }

                user.UserState = Application.UserState.Deleted;                
                this.userRepo.UpdateUser(user);
                return true;
                
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<List<CommunityDto>> SearchCommunity(string searchKey, int loggedUser)
        {
            try
            {
                Expression<Func<User, bool>> query = s => s.Name.Contains(searchKey,StringComparison.OrdinalIgnoreCase) && s.UserState != Application.UserState.Deleted; //added draft sprint exlusion here;

                List<User> userList = await this.userRepo.GetCommunity(query);
                
                User currentUser = await this.userRepo.GetUserById(loggedUser);

                List <CommunityDto> communityList = new List<CommunityDto>();

                foreach (User user in userList)
                {
                    CommunityDto community = new CommunityDto();
                    community.UserId = user.Id;
                    community.Name = user.Name;
                    community.ProfilePicture = user.ProfilePicture;
                                      
                    var friends = await this.FriendService.AllFriends(user.Id);
                    community.IsFriendOfMine = friends != null && friends.Count > 0;
                    
                    #region commented

                    // community.FriendDto = new List<FriendDto>();


                    //foreach (var friend in user.friendsAccepted)
                    //{
                    //    community.FriendDto.Add(new FriendDto(
                    //    friend.AcceptedUser.Id,
                    //    friend.AcceptedUser.Name,
                    //    friend.AcceptedUser.ProfilePicture,
                    //    friend.AcceptedUser.Code,
                    //    friend.AcceptedUser.Email,
                    //    friend.AcceptedUser.City,
                    //    friend.AcceptedUser.Country,
                    //    friend.AcceptedUser.CountryCode,
                    //    friend.AcceptedUser.ColorCode,
                    //    friend.CreatedDate,
                    //    friend.AcceptedUser.UserShareType,
                    //    friend.AcceptedUser.Id == loggedUser
                    //    ));

                    //} 
                    #endregion

                    communityList.Add(community);
                }

                return communityList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get User Role Info
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<List<RolesDto>> GetUserRoleInfo(int userID)
        {
            try
            {
               return await this.userRepo.GetUserRoleInfo(userID);
                
            }
            catch (System.Exception Ex)
            {
                throw Ex;
            }
        }


    }
}