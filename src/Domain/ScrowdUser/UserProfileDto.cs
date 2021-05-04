using SprintCrowd.BackEnd.Domain.Friend;
using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
using System;
using SprintCrowd.Domain.Achievement;
using System.Collections.Generic;

namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    /// <summary>
    /// User  Profile Dto data transfer object
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// Initalize <see cref="UserProfileDto"> class </see>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="profilePicture"></param>
        /// <param name="countryCode"></param>
        /// <param name="joinedDate"></param>
        public UserProfileDto(int userId, string name,string description, string profilePicture, string countryCode , DateTime? joinedDate , List<FriendDto> friendDto , SprintStatisticDto sprintStatisticDto, List<AchievementDto> achievementDto)
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
        }

        /// <summary>
        /// Gets user id
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets user name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets user description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets user profile picture
        /// </summary>
        public string ProfilePicture { get; }

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
        /// Get user FriendDto
        /// </summary>
        public List<FriendDto> FriendDto { get; }

        /// <summary>
        /// Get user SprintStatisticDto
        /// </summary>
        public SprintStatisticDto SprintStatisticDto { get; }

        /// <summary>
        /// Get user AchievementDto
        /// </summary>
        public List<AchievementDto> AchievementDto { get; }
    }
}