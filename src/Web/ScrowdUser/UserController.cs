namespace SprintCrowd.Web.ScrowdUser
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Web.ScrowdUser.Models;
    using SprintCrowd.BackEnd.Web.ScrowdUser;

    /// <summary>
    /// User controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
   // [Authorize]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Initialize <see cref="UserController"> class </see>
        /// </summary>
        /// <param name="userService">user service instance</param>
        public UserController(IUserService userService)
        {
            this.UserService = userService;
        }

        private IUserService UserService { get; set; }
        private ScrowdDbContext Context { get; set; }

        /// <summary>
        /// Get authorized user details
        /// </summary>
        [HttpGet("get")]
        public async Task<IActionResult> GetUser()
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            var userResult = await this.UserService.GetUser(authorizedUser.Id);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = userResult,
            };
            return this.Ok(response);
        }


        /// <summary>
        /// Get authorized user details
        /// </summary>
        [HttpPost("getbyEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] UserEmailModel userEmail)
        {
            var user = await this.UserService.getUserByEmail(userEmail.Email);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = user,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Update user activity
        /// </summary>
        [HttpGet("updateActivity")]
        public async Task<IActionResult> UpdateUserActivity()
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            UserActivity activity = await this.UserService.UpdateUserActivity(authorizedUser.Id);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = activity,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get user pereference
        /// </summary>
        /// <returns>user peference</returns>
        [HttpGet("preference")]
        [ProducesResponseType(typeof(SuccessResponse<UserPreferenceDto>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> UserPreference()
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            var result = await this.UserService.GetUserPreference(authorizedUser.Id);
            return this.Ok(new SuccessResponse<UserPreferenceDto>(result));
        }

        /// <summary>
        /// Get uasers by search
        /// </summary>
        /// <returns>user peference</returns>
        [HttpGet("users")]
        [ProducesResponseType(typeof(SuccessResponse<List<UserSelectDto>>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> UsersSearch([FromQuery(Name = "search")] string searchParams)
        {
            var users = await this.UserService.UserSearch(searchParams);
            return this.Ok(new SuccessResponse<List<UserSelectDto>>(users));
        }

        /// <summary>
        /// Update user pereference
        /// </summary>
        /// <returns>user peference</returns>
        [HttpPost("preference")]
        [ProducesResponseType(typeof(SuccessResponse<UserPreferenceDto>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> UpdateUserPreference([FromBody] UserPreferenceModel userPreferenceModel)
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            var result = await this.UserService.UpdateUserPreference(authorizedUser.Id, userPreferenceModel);
            return this.Ok(new SuccessResponse<UserPreferenceDto>(result));
        }

        /// <summary>
        /// Get user settings
        /// </summary>
        /// <returns>user setting</returns>
        [HttpGet("settings")]
        [ProducesResponseType(typeof(SuccessResponse<UserSettingsDto>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> GetUserSettings()
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            var result = await this.UserService.GetUserSettings(authorizedUser.Id);
            return this.Ok(new SuccessResponse<UserSettingsDto>(result));
        }

        /// <summary>
        /// Update user settings
        /// </summary>
        /// <returns>user settings</returns>
        [HttpPost("settings")]
        [ProducesResponseType(typeof(SuccessResponse<UserSettingsDto>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UserSettingsModel userSettingsModel)
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            var result = await this.UserService.UpdateUserSettings(authorizedUser.Id, userSettingsModel);
            return this.Ok(new SuccessResponse<UserSettingsDto>(result));
        }

        /// <summary>
        /// Deactivate user account
        /// </summary>
        [HttpPost("account/deactivate")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> DeactivateUserAccount()
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            await this.UserService.AccountDeactivate(authorizedUser.Id);
            return this.Ok(new SuccessResponse<string>("success"));
        }

        /// <summary>
        /// Loout user account
        /// </summary>
        [HttpPost("account/logout")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
        public async Task<IActionResult> LogoutUserAccount()
        {
            var authorizedUser = await this.User.GetUser(this.UserService);
            await this.UserService.AccountLogout(authorizedUser.Id);
            return this.Ok(new SuccessResponse<string>("success"));
        }

        /// <summary>
        /// Get sprint statistics
        /// </summary>
        [HttpGet("ViewUserProfile")]
        [ProducesResponseType(typeof(SuccessResponse<UserProfileDto>), 200)]
        public async Task<IActionResult> ViewUserProfile()
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.UserService.ViewUserProfile(user.Id);
            return this.Ok(new SuccessResponse<UserProfileDto>(result));
        }

        /// <summary>
        /// Get sprint statistics
        /// </summary>
        [HttpPut("UserProfileEdit")]
        [ProducesResponseType(typeof(SuccessResponse<UserProfileDto>), 200)]
        public async Task<IActionResult> UserProfileEdit(UserProfileDto profileData)
        {
            var result = await this.UserService.UpdateUserProfile(profileData);
            return this.Ok(new SuccessResponse<UserProfileDto>(result));
        }


        /// <summary>
        /// Delete a user profile
        /// <param name="userId">user id</param>
        /// </summary>
        [HttpDelete("UserProfileDelete/{userId}")]       
        public async Task<IActionResult> UserProfileDelete(int userId)
        {
            var result = await this.UserService.DeleteUserProfile(userId);
            return this.Ok(result);
        }
    }
}