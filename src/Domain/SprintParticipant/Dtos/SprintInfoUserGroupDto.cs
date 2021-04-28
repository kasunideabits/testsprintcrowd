using System;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// Sprint info class
    /// </summary>
    public class SprintInfoUserGroupDto
    {
        /// <summary>
        /// Initialize <see cref="SprintInfoUserGroupDto"> class </see>
        /// </summary>
        /// <param name="id">sprint id</param>
        /// <param name="name">sprint name</param>
        /// <param name="distance">sprint distance</param>
        /// <param name="sprintType">sprint type</param>
        /// <param name="startTime">start date time</param>
        /// <param name="startTime">user group</param>
        /// <param name="sprintCreator">sprint creator or not</param>
        public SprintInfoUserGroupDto(int id, string name, int distance, DateTime startTime, int sprintType,string userGroup, bool sprintCreator = false)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.SprintType = sprintType;
            this.SprintCreator = sprintCreator;
            this.UserGroup = userGroup;
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
        /// Get UserGroup
        /// </summary>
        public string UserGroup { get; }
    }
}