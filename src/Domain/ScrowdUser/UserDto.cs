using System;
using System.Collections.Generic;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    /// <summary>
    /// User info data transfer object
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Initalize <see cref="UserDto"> class </see>
        /// </summary>
        /// <param name="userId">unique id for the user</param>
        /// <param name="name">name for user</param>
        /// <param name="profilePicture">profile picture url for user</param>
        /// <param name="userCode">profile picture url for user</param>
        public UserDto(int userId, string name, string profilePicture, string userCode, DateTime joinedDate, string description = "", string countryCode = "")
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = userCode;
            this.Description = description;
            this.CountryCode = countryCode;
            this.JoinedDate = joinedDate;
           
        }

        public UserDto()
        {

        }
        public UserDto(User user, List<RolesDto> userRoles = null)
        {
            this.UserId = user.Id;
            this.Name = user.Name;
            this.ProfilePicture = user.ProfilePicture;
            this.Code = user.Code;
            this.Description = user.Description;
            this.CountryCode = user.CountryCode;
            this.JoinedDate = user.CreatedDate;
            this.Email = user.Email;
            this.UserShareType = user.UserShareType;
            this.UserRoles = userRoles;
        }

        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets user name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets user profile picture
        /// </summary>
        public string ProfilePicture { get; }

        /// <summary>
        /// Get user coed
        /// </summary>
        /// <value></value>
        public string Code { get; }

        /// <summary>
        /// Gets user description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Get user country code
        /// </summary>
        /// <value></value>
        public string CountryCode { get; }

        /// <summary>
        /// Get user joined date
        /// </summary>
        /// <value></value>
        public DateTime? JoinedDate { get; }

        /// <summary>
        /// Get user email
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// Share route statistic details with
        /// </summary>
        public UserShareType UserShareType { get; set; }

        /// <summary>
        /// Share route statistic details with
        /// </summary>
        public List<RolesDto> UserRoles { get; set; }
    }
}