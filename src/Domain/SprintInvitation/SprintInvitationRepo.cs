namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Storing and retirve sprint invitations
    /// </summary>
    public class SprintInvitationRepo : ISprintInvitationRepo
    {
        /// <summary>
        /// Initialize <see cref="SprintInvitationRepo"> class</see>
        /// </summary>
        /// <param name="context">database context</param>
        public SprintInvitationRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }
        private ScrowdDbContext Context { get; }

        /// <summary>
        /// Add sprint invitaiton
        /// </summary>
        /// <param name="inviterId">inviter user id</param>
        /// <param name="inviteeId">invite user id</param>
        /// <param name="sprintId">sprint id</param>
        /// <returns> sprint invite id </returns>
        public async Task<int> Invite(int inviterId, int inviteeId, int sprintId)
        {
            SprintInvite invite = new SprintInvite()
            {
                SprintId = sprintId,
                InviterId = inviterId,
                InviteeId = inviteeId
            };
            var result = await this.Context.SprintInvite.AddAsync(invite);
            return result.Entity.Id;
        }

        /// <summary>
        /// Add sprint notification to notifcation table
        /// </summary>
        /// <param name="senderId">Sender user id</param>
        /// <param name="receiverId">Receiver user id</param>
        /// <param name="sprintInviteId">Sprint invite id</param>
        public async Task AddNotification(int senderId, int receiverId, int sprintInviteId)
        {
            try
            {
                Notification notification = new Notification()
                {
                    NotiticationType = NotificationType.SprintInvitation,
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    SprintInviteId = sprintInviteId,
                    SendTime = DateTime.UtcNow,
                    IsRead = false,
                };
                await this.Context.Notification.AddAsync(notification);
            }
            catch (System.Exception ex)
            {
                throw new Application.ApplicationException(ex.Message.ToString());
            }

        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public Task SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}