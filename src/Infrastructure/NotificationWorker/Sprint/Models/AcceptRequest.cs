using SprintCrowd.BackEnd.Application;
using System;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    public class AcceptRequest
    {
        /// <summary>
        ///Initialize <see cref="AcceptRequest"/> class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="profilePicture"></param>
        /// <param name="code"></param>
        /// <param name="email"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="countryCode"></param>
        /// <param name="colorCode"></param>
        /// <param name="createdDate"></param>
        public AcceptRequest(int id, string name, string profilePicture, string code, string email, string city,  string country ,string countryCode ,string colorCode ,DateTime createdDate,int requestSenderId, bool isCommunity)
        {
            this.Id = id;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = code;
            this.Email = email;
            this.City = city;
            this.Country = country;
            this.CountryCode = countryCode;
            this.ColorCode = colorCode;
            this.CreatedDate = createdDate;
            this.RequestSenderId = requestSenderId;
            this.IsCommunity = isCommunity;
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// ProfilePicture
        /// </summary>
        public string ProfilePicture { get; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; }

        /// <summary>
        /// CountryCode
        /// </summary>
        public string CountryCode { get; }

        /// <summary>
        /// ColorCode
        /// </summary>
        public string ColorCode { get; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; }

        /// <summary>
        /// Request Sender UserId
        /// </summary>
        public int RequestSenderId { get; }

        public bool IsCommunity { get; }
    }
}