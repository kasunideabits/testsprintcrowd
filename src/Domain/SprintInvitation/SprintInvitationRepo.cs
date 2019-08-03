namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    public class SprintInvitationRepo : ISprintInvitationRepo
    {
        public SprintInvitationRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }
        private ScrowdDbContext Context { get; }

        public async Task AddNotification(int senderId, int receiverId, int sprintId)
        {
            try
            {
                Notification notification = new Notification()
                {
                    NotiticationType = NotificationType.SprintInvitation,
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    SprintId = sprintId,
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

        public async Task Invite(int inviterId, int inviteeId, int sprintId)
        {
            SprintInvite invite = new SprintInvite()
            {
                SprintId = sprintId,
                InviterId = inviterId,
                InviteeId = inviteeId
            };
            await this.Context.SprintInvite.AddAsync(invite);
            return;
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}