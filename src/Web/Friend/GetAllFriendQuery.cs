namespace SprintCrowd.BackEnd.Web.Friend
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Get all firend query string attributes
    /// </summary>
    public class GetAllFriendQuery
    {
        /// <summary>
        /// Gets or set Request status for friend request.default
        /// <see cref="RequestStatus"> Pendding </see>
        /// </summary>
        public FriendRequestStatus? RequestStatus { get; set; }
    }
}