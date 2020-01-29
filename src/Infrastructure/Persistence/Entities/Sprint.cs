namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;

    /// <summary>
    /// Sprint Model.
    /// </summary>
    public class Sprint : BaseEntity
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id for the event.</value>
        public int Id { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event name.</value>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>distance for the event.</value>
        public int Distance { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>created by the user.</value>
        public User CreatedBy { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event date start time.</value>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event type private or public.</value>
        public int Type { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>event status, started or not.</value>
        public int Status { get; set; }

        /// <summary>
        /// gets or sets location.
        /// </summary>
        [MaxLength(100)]
        public string Location { get; set; }

        /// <summary>
        /// Number of participant
        /// </summary>
        /// <value>Number of pariticipants for the event</value>
        public int NumberOfParticipants { get; set; }

        /// <summary>
        /// Sprint created date
        /// </summary>
        /// <value>sprint created date</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>participants of the event.</value>
        public virtual List<SprintParticipant> Participants { get; set; }

        // <summary>
        /// get or ser sprint invite reference
        /// </summary>
        /// <value></value>
        public virtual List<SprintInvite> SprintInvites { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>Influencer Availability.</value>
        public bool InfluencerAvailability { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>Influencer Email.</value>
        [MaxLength(50)]
        public string InfluencerEmail { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>draft event status, drafted or not.</value>
        public int DraftEvent { get; set; }

    }
}