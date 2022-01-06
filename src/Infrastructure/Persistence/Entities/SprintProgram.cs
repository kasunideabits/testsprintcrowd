using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    public class SprintProgram : BaseEntity
    {
        public SprintProgram() { }
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id for the event.</value>
        public SprintProgram(SprintProgram sprintProgram)
        {
            this.Id = sprintProgram.Id;
            this.Name = sprintProgram.Name;
            this.Description = sprintProgram.Description;
            this.Duration = sprintProgram.Duration;
            this.IsPrivate = sprintProgram.IsPrivate;
            this.ImageUrl = sprintProgram.ImageUrl;
            this.GetSocialLink = sprintProgram.GetSocialLink;
            this.ProgramCode = sprintProgram.ProgramCode;
            this.StartDate = sprintProgram.StartDate;
            this.CreatedBy = sprintProgram.CreatedBy;
            
        }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>Auto Generated ID.</value>
        public int Id { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program name.</value>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program Description.</value>
        public string Description { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program duration.</value>
        public int Duration { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program status, Public or Private.</value>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// imageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Get Social Link
        /// </summary>
        public string GetSocialLink { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program ProgramCode.</value>
        public string ProgramCode { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program StartDate.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>created by the user.</value>
        public User CreatedBy { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>ststus of the program.</value>
        public int Status { get; set; }
    }

}