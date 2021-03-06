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
        public SprintInfo(int id, string name, int distance, DateTime startTime, int sprintType, bool isInfluencerEventParticipant, bool sprintCreator = false, bool isNarrationsOn = true)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.SprintType = sprintType;
            this.SprintCreator = sprintCreator;
            this.ISNarrationsOn = isNarrationsOn;
            this.IsInfluencerEventParticipant = isInfluencerEventParticipant;
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
            this.ISNarrationsOn = sprint.IsNarrationsOn;
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
        /// Mute narrations
        /// </summary>
        public bool ISNarrationsOn { get; }
    }
}