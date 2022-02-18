﻿using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Sprint.Dtos
{
    public class SprintProgramDto
    {
        public SprintProgramDto() { }
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id for the event.</value>
        public SprintProgramDto(SprintProgram sprintProgram , List<SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint> programSprints)
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
            this.ProgramSprints = programSprints;
            this.IsPublish = sprintProgram.IsPublish;
            this.PromotionalText = sprintProgram.PromotionalText;
            this.EndDate = sprintProgram.StartDate.AddDays(sprintProgram.Duration * 7);
            this.Events = programSprints.Count;
            this.CreatedBy = sprintProgram.CreatedBy;
            this.IsPromoteInApp = sprintProgram.IsPromoteInApp;
            this.Status = sprintProgram.Status;
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

        // <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>ststus of the program.</value>
        public int Status { get; set; }

        /// <summary>
        /// Is Publish
        /// </summary>
        public bool IsPublish { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>Is Promote In App.</value>
        /// 
        public bool IsPromoteInApp { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program promotional Text.</value>
        public string PromotionalText { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program EndDate.</value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>Number of Events in Program.</value>
        public int Events { get; set; }


        /// <summary>
        /// Sprint list within the program
        /// </summary>
        public List<SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint> ProgramSprints { get; set; }


        
    }
}
