﻿namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;

    public interface ISprintInvitationRepo
    {
        /// <summary>
        /// Add sprint invitaiton
        /// </summary>
        /// <param name="inviterId">inviter user id</param>
        /// <param name="inviteeId">invite user id</param>
        /// <param name="sprintId">sprint id</param>
        Task Invite(int inviterId, int inviteeId, int sprintId);

        /// <summary>
        /// Add sprint notification to notifcation table
        /// </summary>
        /// <param name="senderId">Sender user id</param>
        /// <param name="receiverId">Receiver user id</param>
        /// <param name="sprintId">Sprint id</param>
        Task AddNotification(int senderId, int receiverId, int sprintId);

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        void SaveChanges();
    }
}