using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Sprint.Dtos
{
    public class ProgramSprintScheduleDto
    {
        private List<ProgramSprint> programSprints = new List<ProgramSprint>();
        public ProgramSprintScheduleDto()
        {
         
        }
        /// <summary>
        /// Program start date of the the week
        /// </summary>
        public DateTime WeekDate { get; set; }

        /// <summary>
        ///  Program Sprints List
        /// </summary>
        public List<ProgramSprint> ProgramSprints
        {
            get
            {
                if (this.programSprints != null)
                    return this.programSprints;
                else
                    this.programSprints = new List<ProgramSprint>();
                return this.programSprints;
            }
            set
            {
                this.programSprints = value;
            }

        }
    }

    public class ProgramSprint
    {
        public ProgramSprint(int sprintId,string sprintName, int sprintDistance ,DateTime sprintStartTime , string imageUrl , bool isTimeBased, TimeSpan timeBasedDuration)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.SprintDistance = sprintDistance;
            this.SprintStartTime = sprintStartTime;
            this.ImageUrl = imageUrl;
            this.IsTimeBased = isTimeBased;
            this.TimeBasedDuration = timeBasedDuration;
        }

        public int SprintId { get; set; }
        public string SprintName { get; set; }
        public int SprintDistance { get; set; }
        public DateTime SprintStartTime { get; set; }
        public string ImageUrl { get; set; }

        public bool IsTimeBased { get; set; }

        public TimeSpan TimeBasedDuration { get; set; }
    }

    public class ProgramSprintScheduleEvents
    {
        
        public ProgramSprintScheduleEvents(List<ProgramSprintScheduleDto> events)
        {
            this.Events = events;
        }


        /// <summary>
        ///  Program Sprints Events List
        /// </summary>
        public List<ProgramSprintScheduleDto> Events
        {
            get;
            set;
        }
    }
}
