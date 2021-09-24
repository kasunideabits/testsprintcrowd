using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Sprint Participant Members
    /// </summary>
    public class SprintParticipantMembers : BaseEntity
    {

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set Sprint Id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        ///  Gets or set User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Member Id
        /// </summary>
        public string MemberId { get; set; }
    }
}
