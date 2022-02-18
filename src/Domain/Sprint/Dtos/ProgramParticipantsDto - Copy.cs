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
        
        public ProgramParticipantsDto(string name , string description, int duration, DateTime startDate, DateTime endDate,int participants, List<SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintParticipant> programParticipants)
        {
            this.Name = name;
            this.Description = description;
            this.Duration = duration;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Participants = participants;
            this.ProgramParticipants = programParticipants;
            
          
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
        public List<SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintParticipant> ProgramParticipants { get; set; }

        /// <summary>
        /// Number of participants
        /// </summary>
        public int Participants { get; set; }

        
    }
}
