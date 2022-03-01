using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Sprint.Dtos
{
    public class ProgramParticipantsDto
    {
        public ProgramParticipantsDto() { }
        
        public ProgramParticipantsDto(string name , string description, int duration, DateTime startDate, DateTime endDate,int participants, List<ParticipantInfoDto> programParticipants ,bool isUserJoined, bool isPromoteInApp , string promotionalText, int events,int programParticipantCount, bool isPrivate)
        {
            this.Name = name;
            this.Description = description;
            this.Duration = duration;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Participants = participants;
            this.ProgramParticipants = programParticipants;
            this.IsUserJoined = isUserJoined;
            this.IsPromoteInApp = isPromoteInApp;
            this.PromotionalText = promotionalText;
            this.Events = events;
            this.ProgramParticipantCount = programParticipantCount;
            this.IsPrivate = isPrivate

;

        }


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
        /// <value>program StartDate.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>program EndDate.</value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Participantt list within the program
        /// </summary>
        public List<ParticipantInfoDto> ProgramParticipants { get; set; }

        /// <summary>
        /// Number of participants
        /// </summary>
        public int Participants { get; set; }

        /// <summary>
        /// Is User Joined
        /// </summary>
        public bool IsUserJoined { get; set; }
        
        /// <summary>
        /// Is Promote In App
        /// </summary>
        public bool IsPromoteInApp { get; set; }

        /// <summary>
        /// Promotional Text
        /// </summary>
        public string PromotionalText { get; set; }

        /// <summary>
        /// Events
        /// </summary>
        public int Events { get; set; }

        /// <summary>
        /// Program Participant Count
        /// </summary>
        public int ProgramParticipantCount { get; set; }

        /// <summary>
        /// Is Private
        /// </summary>
        public bool IsPrivate { get; set; }
    }
}
