using System;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// Sprint info class
    /// </summary>
    public class SprintInfo
    {
        /// <summary>
        /// Initialize <see cref="SprintInfo"> class </see>
        /// </summary>
        /// <param name="id">sprint id</param>
        /// <param name="name">sprint name</param>
        /// <param name="distance">sprint distance</param>
        /// <param name="sprintType">sprint type</param>
        /// <param name="startTime">start date time</param>
        /// <param name="sprintCreator">sprint creator or not</param>
        public SprintInfo(int id, string name, int distance, DateTime startTime, int sprintType, bool isInfluencerEventParticipant,
                                     bool sprintCreator = false, bool isTimeBased = false,
                                     TimeSpan durationForTimeBasedEvent = default(TimeSpan),
                                     string descriptionForTimeBasedEvent = null, bool isNarrationsOn = false, string coHost = "") 
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.SprintType = sprintType;
            this.SprintCreator = sprintCreator;
            this.IsInfluencerEventParticipant = isInfluencerEventParticipant;
            this.IsTimeBased = isTimeBased;
            this.DurationForTimeBasedEvent = durationForTimeBasedEvent;
            this.DescriptionForTimeBasedEvent = descriptionForTimeBasedEvent;
            this.IsNarrationsOn = isNarrationsOn;
            this.CoHost = coHost;
        }

        public SprintInfo(Infrastructure.Persistence.Entities.Sprint sprint, bool sprintCreator = false, bool isInfluencerEventParticipant = false)
        {
            this.Id = sprint.Id;
            this.Name = sprint.Name;
            this.Distance = sprint.Distance;
            this.StartTime = sprint.StartDateTime;
            this.SprintType = sprint.Type;
            this.SprintCreator = sprintCreator;
            this.IsInfluencerEventParticipant = isInfluencerEventParticipant;
           
        }

        /// <summary>
        /// Gets sprint id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets sprint name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets sprint distance
        /// </summary>
        public int Distance { get; }

        /// <summary>
        /// Gets sprint start time
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Gets sprint type, public or private
        /// </summary>
        public int SprintType { get; }

        /// <summary>
        ///  Get sprint creator or not
        /// </summary>
        public bool SprintCreator { get; }

        /// <summary>
        /// Get Is Influencer Event Participant
        /// </summary>
        public bool IsInfluencerEventParticipant { get; }

        /// <summary>
        /// Get if event is timebased or not
        /// </summary>
        public bool IsTimeBased { get; }

        /// <summary>
        /// Get the duration of the timebased event
        /// </summary>
        public TimeSpan DurationForTimeBasedEvent { get; }

        /// <summary>
        /// Get the description of the timebased event
        /// </summary>
        public string DescriptionForTimeBasedEvent { get; }

        /// Get Is Influencer Event Participant
        /// </summary>
        public bool IsNarrationsOn { get; }

        /// Get Co Host of Sprint
        /// </summary>
        public string CoHost { get; }
    }
}