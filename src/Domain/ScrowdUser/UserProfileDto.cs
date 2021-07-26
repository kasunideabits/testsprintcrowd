using SprintCrowd.BackEnd.Domain.Friend;
using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
using System;
using SprintCrowd.Domain.Achievement;
using System.Collections.Generic;
using SprintCrowd.BackEnd.Application;
using SprintCrowdBackEnd.Domain.Sprint.Dtos;

namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    /// <summary>
    /// User  Profile Dto data transfer object
    /// </summary>
    public class UserProfileDto
    {

        public UserProfileDto() { }

        /// <summary>
        /// Initalize <see cref="UserProfileDto"> class </see>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="profilePicture"></param>
        /// <param name="countryCode"></param>
        /// <param name="joinedDate"></param>
        public UserProfileDto(int userId, string name,string description, string profilePicture, string countryCode , DateTime? joinedDate , List<FriendDto> friendDto , SprintStatisticDto sprintStatisticDto, List<AchievementDto> achievementDto, UserShareType userShareType, List<SprintWithParticipantProfile> participants = null, int inviteId = 0)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Description = description;
            this.CountryCode = countryCode;
            this.JoinedDate = joinedDate;
            this.FriendDto = friendDto;
            this.SprintStatisticDto = sprintStatisticDto;
            this.AchievementDto = achievementDto;
            this.UserShareType = userShareType;
            this.SprintWithParticipantProfiles = participants;
            this.InviteId = inviteId;
        }

        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets user description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets user profile picture
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Get user country code
        /// </summary>
        /// <value></value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Get user country 
        /// </summary>
        /// <value></value>
        public string Country { get; set; }

        /// <summary>
        /// Get user joined date
        /// </summary
        /// <value></value>
        public DateTime? JoinedDate { get; set; }

        /// <summary>
        /// Get user FriendDto
        /// </summary>
        public List<FriendDto> FriendDto { get; set; }

        /// <summary>
        /// Get user SprintStatisticDto
        /// </summary>
        public SprintStatisticDto SprintStatisticDto { get; set; }

        /// <summary>
        /// Get user AchievementDto
        /// </summary>
        public List<AchievementDto> AchievementDto { get; set; }

        /// <summary>
        /// Share route statistic details with
        /// </summary>
        public UserShareType UserShareType { get; set; }

        /// <summary>
        /// Sprint participant profile with sprint data
        /// </summary>
        public List<SprintWithParticipantProfile> SprintWithParticipantProfiles { get; set; }

        /// <summary>
        /// Invite Id
        /// </summary>
        public int InviteId { get; set; }
    }
}