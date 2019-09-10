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
        /// <param name="startTime">start date time</param>
        /// <param name="sprintCreator">sprint creator or not</param>
        public SprintInfo(int id, string name, int distance, DateTime startTime, bool sprintCreator = false)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.SprintCreator = sprintCreator;
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
        ///  Get sprint creator or not
        /// </summary>
        public bool SprintCreator { get; }
    }
}