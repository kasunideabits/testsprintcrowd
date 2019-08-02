using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Web.Friend
{
    /// <summary>
    /// Get a firend query string attributes
    /// </summary>
    public class GetFriendQuery
    {
        /// <summary>
        /// Gets or set friend id for lookup
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Gets or set request status for lookup
        /// default is  <see cref="RequestStatus"> Pendding </see>
        /// </summary>
        public FriendRequestStatus? RequestStatus { get; set; }
    }
}