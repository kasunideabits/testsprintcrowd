using System;
using System.ComponentModel;

namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    /// <summary>
    /// SprintReport Model.
    /// </summary>
    public class ReportItemDto
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event name.</value>
        [DisplayName("Sprint Name")]
        public string SprintName { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event start date.</value>
        [DisplayName("Start Date and Time")]
        public DateTime StartDate { get; set; }

        // /// <summary>
        // /// gets or sets value.
        // /// </summary>
        // /// <value>event start time.</value>
        // [DisplayName("Start Time")]
        // public DateTime StartTime { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event distnace</value>
        [DisplayName("Distance(Meters)")]
        public int Distance { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event name.</value>
        [DisplayName("Sprint Type")]
        public string SprintType { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event participants count.</value>
        [DisplayName("Joined")]
        public int ParticipantsCount { get; set; }


        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>participants count who marked their attendance.</value>
        [DisplayName("Marked Attendance")]
        public int ParticipantsMarkedAttendance { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>participants count who finished event.</value>
        [DisplayName("Completed")]
        public int ParticipantsFinishedSprint { get; set; }
    }
}
