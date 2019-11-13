using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class SnakeCaseSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievement_User_UserId",
                table: "Achievement");

            migrationBuilder.DropForeignKey(
                name: "FK_FirebaseToken_User_UserId",
                table: "FirebaseToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Frineds_User_AcceptedUserId",
                table: "Frineds");

            migrationBuilder.DropForeignKey(
                name: "FK_Frineds_User_SharedUserId",
                table: "Frineds");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Achievement_AchievementId1",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_SprintInvite_SprintInviteId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Sprint_User_CreatedById",
                table: "Sprint");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintInvite_User_InviteeId",
                table: "SprintInvite");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintInvite_User_InviterId",
                table: "SprintInvite");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintInvite_Sprint_SprintId",
                table: "SprintInvite");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintParticipant_Sprint_SprintId",
                table: "SprintParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintParticipant_User_UserId",
                table: "SprintParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_User_AccessToken_AccessTokenId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_Notification_NotificationId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_User_ReceiverId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_User_SenderId",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sprint",
                table: "Sprint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Frineds",
                table: "Frineds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Achievement",
                table: "Achievement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SprintParticipant",
                table: "SprintParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SprintInvite",
                table: "SprintInvite");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SprintInvite_InviterId_InviteeId_SprintId",
                table: "SprintInvite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FirebaseToken",
                table: "FirebaseToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppDownloads",
                table: "AppDownloads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessToken",
                table: "AccessToken");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Sprint",
                newName: "sprint");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "notification");

            migrationBuilder.RenameTable(
                name: "Frineds",
                newName: "frineds");

            migrationBuilder.RenameTable(
                name: "Achievement",
                newName: "achievement");

            migrationBuilder.RenameTable(
                name: "UserNotification",
                newName: "user_notification");

            migrationBuilder.RenameTable(
                name: "SprintParticipant",
                newName: "sprint_participant");

            migrationBuilder.RenameTable(
                name: "SprintInvite",
                newName: "sprint_invite");

            migrationBuilder.RenameTable(
                name: "FirebaseToken",
                newName: "firebase_token");

            migrationBuilder.RenameTable(
                name: "AppDownloads",
                newName: "app_downloads");

            migrationBuilder.RenameTable(
                name: "AccessToken",
                newName: "access_token");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "user",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "user",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "user",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "user",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "user",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "user",
                newName: "user_type");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "user",
                newName: "profile_picture");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "user",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "LanguagePreference",
                table: "user",
                newName: "language_preference");

            migrationBuilder.RenameColumn(
                name: "FacebookUserId",
                table: "user",
                newName: "facebook_user_id");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "user",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "user",
                newName: "country_code");

            migrationBuilder.RenameColumn(
                name: "ColorCode",
                table: "user",
                newName: "color_code");

            migrationBuilder.RenameColumn(
                name: "AccessTokenId",
                table: "user",
                newName: "access_token_id");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "user",
                newName: "IX_user_email");

            migrationBuilder.RenameIndex(
                name: "IX_User_Code",
                table: "user",
                newName: "IX_user_code");

            migrationBuilder.RenameIndex(
                name: "IX_User_AccessTokenId",
                table: "user",
                newName: "IX_user_access_token_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "sprint",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "sprint",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "sprint",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "sprint",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "sprint",
                newName: "distance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sprint",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "sprint",
                newName: "start_date_time");

            migrationBuilder.RenameColumn(
                name: "NumberOfParticipants",
                table: "sprint",
                newName: "number_of_participants");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "sprint",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "InfluencerEmail",
                table: "sprint",
                newName: "influencer_email");

            migrationBuilder.RenameColumn(
                name: "InfluencerAvailability",
                table: "sprint",
                newName: "influencer_availability");

            migrationBuilder.RenameColumn(
                name: "DraftEvent",
                table: "sprint",
                newName: "draft_event");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "sprint",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "sprint",
                newName: "created_by_id");

            migrationBuilder.RenameIndex(
                name: "IX_Sprint_CreatedById",
                table: "sprint",
                newName: "IX_sprint_created_by_id");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "notification",
                newName: "distance");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "notification",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "notification",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "notification",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "UpdatorId",
                table: "notification",
                newName: "updator_id");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "notification",
                newName: "start_date_time");

            migrationBuilder.RenameColumn(
                name: "SprintType",
                table: "notification",
                newName: "sprint_type");

            migrationBuilder.RenameColumn(
                name: "SprintStatus",
                table: "notification",
                newName: "sprint_status");

            migrationBuilder.RenameColumn(
                name: "SprintNotificationType",
                table: "notification",
                newName: "sprint_notification_type");

            migrationBuilder.RenameColumn(
                name: "SprintName",
                table: "notification",
                newName: "sprint_name");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "notification",
                newName: "sprint_id");

            migrationBuilder.RenameColumn(
                name: "NumberOfParticipants",
                table: "notification",
                newName: "number_of_participants");

            migrationBuilder.RenameColumn(
                name: "SprintInviteId",
                table: "notification",
                newName: "sprint_invite_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "notification",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "notification",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "AchievementId1",
                table: "notification",
                newName: "achievement_id1");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "notification",
                newName: "requester_id");

            migrationBuilder.RenameColumn(
                name: "AccepterId",
                table: "notification",
                newName: "accepter_id");

            migrationBuilder.RenameColumn(
                name: "AchievementId",
                table: "notification",
                newName: "achievement_id");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SprintInviteId",
                table: "notification",
                newName: "IX_notification_sprint_invite_id");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_AchievementId1",
                table: "notification",
                newName: "IX_notification_achievement_id1");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "frineds",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedTime",
                table: "frineds",
                newName: "updated_time");

            migrationBuilder.RenameColumn(
                name: "SharedUserId",
                table: "frineds",
                newName: "shared_user_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "frineds",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "frineds",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "AcceptedUserId",
                table: "frineds",
                newName: "accepted_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Frineds_SharedUserId",
                table: "frineds",
                newName: "IX_frineds_shared_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Frineds_AcceptedUserId",
                table: "frineds",
                newName: "IX_frineds_accepted_user_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "achievement",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "achievement",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "achievement",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "achievement",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "achievement",
                newName: "created_date");

            migrationBuilder.RenameIndex(
                name: "IX_Achievement_UserId",
                table: "achievement",
                newName: "IX_achievement_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_notification",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "user_notification",
                newName: "sender_id");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "user_notification",
                newName: "receiver_id");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "user_notification",
                newName: "notification_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "user_notification",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "user_notification",
                newName: "created_date");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_SenderId",
                table: "user_notification",
                newName: "IX_user_notification_sender_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_ReceiverId",
                table: "user_notification",
                newName: "IX_user_notification_receiver_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_NotificationId",
                table: "user_notification",
                newName: "IX_user_notification_notification_id");

            migrationBuilder.RenameColumn(
                name: "Stage",
                table: "sprint_participant",
                newName: "stage");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sprint_participant",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "sprint_participant",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "sprint_participant",
                newName: "sprint_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "sprint_participant",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "sprint_participant",
                newName: "created_date");

            migrationBuilder.RenameIndex(
                name: "IX_SprintParticipant_UserId_SprintId",
                table: "sprint_participant",
                newName: "IX_sprint_participant_user_id_sprint_id");

            migrationBuilder.RenameIndex(
                name: "IX_SprintParticipant_SprintId",
                table: "sprint_participant",
                newName: "IX_sprint_participant_sprint_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "sprint_invite",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sprint_invite",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "sprint_invite",
                newName: "sprint_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "sprint_invite",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "InviterId",
                table: "sprint_invite",
                newName: "inviter_id");

            migrationBuilder.RenameColumn(
                name: "InviteeId",
                table: "sprint_invite",
                newName: "invitee_id");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "sprint_invite",
                newName: "created_date");

            migrationBuilder.RenameIndex(
                name: "IX_SprintInvite_SprintId",
                table: "sprint_invite",
                newName: "IX_sprint_invite_sprint_id");

            migrationBuilder.RenameIndex(
                name: "IX_SprintInvite_InviteeId",
                table: "sprint_invite",
                newName: "IX_sprint_invite_invitee_id");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "firebase_token",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "firebase_token",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "firebase_token",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "firebase_token",
                newName: "last_updated");

            migrationBuilder.RenameIndex(
                name: "IX_FirebaseToken_UserId",
                table: "firebase_token",
                newName: "IX_firebase_token_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "app_downloads",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "app_downloads",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "DevicePlatform",
                table: "app_downloads",
                newName: "device_platform");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "app_downloads",
                newName: "device_id");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "app_downloads",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "access_token",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "access_token",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "access_token",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "access_token",
                newName: "created_date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sprint",
                table: "sprint",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notification",
                table: "notification",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_frineds",
                table: "frineds",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_achievement",
                table: "achievement",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_notification",
                table: "user_notification",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sprint_participant",
                table: "sprint_participant",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sprint_invite",
                table: "sprint_invite",
                column: "id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_sprint_invite_inviter_id_invitee_id_sprint_id",
                table: "sprint_invite",
                columns: new[] { "inviter_id", "invitee_id", "sprint_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_firebase_token",
                table: "firebase_token",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_app_downloads",
                table: "app_downloads",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_access_token",
                table: "access_token",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_achievement_user_user_id",
                table: "achievement",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_firebase_token_user_user_id",
                table: "firebase_token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_accepted_user_id",
                table: "frineds",
                column: "accepted_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_shared_user_id",
                table: "frineds",
                column: "shared_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_achievement_achievement_id1",
                table: "notification",
                column: "achievement_id1",
                principalTable: "achievement",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_sprint_invite_sprint_invite_id",
                table: "notification",
                column: "sprint_invite_id",
                principalTable: "sprint_invite",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_user_created_by_id",
                table: "sprint",
                column: "created_by_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_invite_user_invitee_id",
                table: "sprint_invite",
                column: "invitee_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_invite_user_inviter_id",
                table: "sprint_invite",
                column: "inviter_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_invite_sprint_sprint_id",
                table: "sprint_invite",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_participant_sprint_sprint_id",
                table: "sprint_participant",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_participant_user_user_id",
                table: "sprint_participant",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_access_token_access_token_id",
                table: "user",
                column: "access_token_id",
                principalTable: "access_token",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_notification_notification_notification_id",
                table: "user_notification",
                column: "notification_id",
                principalTable: "notification",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_notification_user_receiver_id",
                table: "user_notification",
                column: "receiver_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_notification_user_sender_id",
                table: "user_notification",
                column: "sender_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_achievement_user_user_id",
                table: "achievement");

            migrationBuilder.DropForeignKey(
                name: "FK_firebase_token_user_user_id",
                table: "firebase_token");

            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_accepted_user_id",
                table: "frineds");

            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_shared_user_id",
                table: "frineds");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_achievement_achievement_id1",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_sprint_invite_sprint_invite_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_user_created_by_id",
                table: "sprint");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_invite_user_invitee_id",
                table: "sprint_invite");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_invite_user_inviter_id",
                table: "sprint_invite");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_invite_sprint_sprint_id",
                table: "sprint_invite");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_participant_sprint_sprint_id",
                table: "sprint_participant");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_participant_user_user_id",
                table: "sprint_participant");

            migrationBuilder.DropForeignKey(
                name: "FK_user_access_token_access_token_id",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_notification_notification_notification_id",
                table: "user_notification");

            migrationBuilder.DropForeignKey(
                name: "FK_user_notification_user_receiver_id",
                table: "user_notification");

            migrationBuilder.DropForeignKey(
                name: "FK_user_notification_user_sender_id",
                table: "user_notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sprint",
                table: "sprint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notification",
                table: "notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_frineds",
                table: "frineds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_achievement",
                table: "achievement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_notification",
                table: "user_notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sprint_participant",
                table: "sprint_participant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sprint_invite",
                table: "sprint_invite");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_sprint_invite_inviter_id_invitee_id_sprint_id",
                table: "sprint_invite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_firebase_token",
                table: "firebase_token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_downloads",
                table: "app_downloads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_access_token",
                table: "access_token");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "sprint",
                newName: "Sprint");

            migrationBuilder.RenameTable(
                name: "notification",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "frineds",
                newName: "Frineds");

            migrationBuilder.RenameTable(
                name: "achievement",
                newName: "Achievement");

            migrationBuilder.RenameTable(
                name: "user_notification",
                newName: "UserNotification");

            migrationBuilder.RenameTable(
                name: "sprint_participant",
                newName: "SprintParticipant");

            migrationBuilder.RenameTable(
                name: "sprint_invite",
                newName: "SprintInvite");

            migrationBuilder.RenameTable(
                name: "firebase_token",
                newName: "FirebaseToken");

            migrationBuilder.RenameTable(
                name: "app_downloads",
                newName: "AppDownloads");

            migrationBuilder.RenameTable(
                name: "access_token",
                newName: "AccessToken");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "User",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "User",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "User",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "User",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "User",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_type",
                table: "User",
                newName: "UserType");

            migrationBuilder.RenameColumn(
                name: "profile_picture",
                table: "User",
                newName: "ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "User",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "language_preference",
                table: "User",
                newName: "LanguagePreference");

            migrationBuilder.RenameColumn(
                name: "facebook_user_id",
                table: "User",
                newName: "FacebookUserId");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "User",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "country_code",
                table: "User",
                newName: "CountryCode");

            migrationBuilder.RenameColumn(
                name: "color_code",
                table: "User",
                newName: "ColorCode");

            migrationBuilder.RenameColumn(
                name: "access_token_id",
                table: "User",
                newName: "AccessTokenId");

            migrationBuilder.RenameIndex(
                name: "IX_user_email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_user_code",
                table: "User",
                newName: "IX_User_Code");

            migrationBuilder.RenameIndex(
                name: "IX_user_access_token_id",
                table: "User",
                newName: "IX_User_AccessTokenId");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Sprint",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Sprint",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Sprint",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Sprint",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "distance",
                table: "Sprint",
                newName: "Distance");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sprint",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "start_date_time",
                table: "Sprint",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "number_of_participants",
                table: "Sprint",
                newName: "NumberOfParticipants");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "Sprint",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "influencer_email",
                table: "Sprint",
                newName: "InfluencerEmail");

            migrationBuilder.RenameColumn(
                name: "influencer_availability",
                table: "Sprint",
                newName: "InfluencerAvailability");

            migrationBuilder.RenameColumn(
                name: "draft_event",
                table: "Sprint",
                newName: "DraftEvent");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Sprint",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "created_by_id",
                table: "Sprint",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_sprint_created_by_id",
                table: "Sprint",
                newName: "IX_Sprint_CreatedById");

            migrationBuilder.RenameColumn(
                name: "distance",
                table: "Notification",
                newName: "Distance");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Notification",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Notification",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Notification",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "updator_id",
                table: "Notification",
                newName: "UpdatorId");

            migrationBuilder.RenameColumn(
                name: "start_date_time",
                table: "Notification",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "sprint_type",
                table: "Notification",
                newName: "SprintType");

            migrationBuilder.RenameColumn(
                name: "sprint_status",
                table: "Notification",
                newName: "SprintStatus");

            migrationBuilder.RenameColumn(
                name: "sprint_notification_type",
                table: "Notification",
                newName: "SprintNotificationType");

            migrationBuilder.RenameColumn(
                name: "sprint_name",
                table: "Notification",
                newName: "SprintName");

            migrationBuilder.RenameColumn(
                name: "sprint_id",
                table: "Notification",
                newName: "SprintId");

            migrationBuilder.RenameColumn(
                name: "number_of_participants",
                table: "Notification",
                newName: "NumberOfParticipants");

            migrationBuilder.RenameColumn(
                name: "sprint_invite_id",
                table: "Notification",
                newName: "SprintInviteId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "Notification",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Notification",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "achievement_id1",
                table: "Notification",
                newName: "AchievementId1");

            migrationBuilder.RenameColumn(
                name: "requester_id",
                table: "Notification",
                newName: "RequesterId");

            migrationBuilder.RenameColumn(
                name: "accepter_id",
                table: "Notification",
                newName: "AccepterId");

            migrationBuilder.RenameColumn(
                name: "achievement_id",
                table: "Notification",
                newName: "AchievementId");

            migrationBuilder.RenameIndex(
                name: "IX_notification_sprint_invite_id",
                table: "Notification",
                newName: "IX_Notification_SprintInviteId");

            migrationBuilder.RenameIndex(
                name: "IX_notification_achievement_id1",
                table: "Notification",
                newName: "IX_Notification_AchievementId1");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Frineds",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_time",
                table: "Frineds",
                newName: "UpdatedTime");

            migrationBuilder.RenameColumn(
                name: "shared_user_id",
                table: "Frineds",
                newName: "SharedUserId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "Frineds",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Frineds",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "accepted_user_id",
                table: "Frineds",
                newName: "AcceptedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_frineds_shared_user_id",
                table: "Frineds",
                newName: "IX_Frineds_SharedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_frineds_accepted_user_id",
                table: "Frineds",
                newName: "IX_Frineds_AcceptedUserId");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Achievement",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Achievement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Achievement",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "Achievement",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Achievement",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_achievement_user_id",
                table: "Achievement",
                newName: "IX_Achievement_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserNotification",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "sender_id",
                table: "UserNotification",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "receiver_id",
                table: "UserNotification",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "notification_id",
                table: "UserNotification",
                newName: "NotificationId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "UserNotification",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "UserNotification",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_user_notification_sender_id",
                table: "UserNotification",
                newName: "IX_UserNotification_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_user_notification_receiver_id",
                table: "UserNotification",
                newName: "IX_UserNotification_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_user_notification_notification_id",
                table: "UserNotification",
                newName: "IX_UserNotification_NotificationId");

            migrationBuilder.RenameColumn(
                name: "stage",
                table: "SprintParticipant",
                newName: "Stage");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SprintParticipant",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "SprintParticipant",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "sprint_id",
                table: "SprintParticipant",
                newName: "SprintId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "SprintParticipant",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "SprintParticipant",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_sprint_participant_user_id_sprint_id",
                table: "SprintParticipant",
                newName: "IX_SprintParticipant_UserId_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_sprint_participant_sprint_id",
                table: "SprintParticipant",
                newName: "IX_SprintParticipant_SprintId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "SprintInvite",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SprintInvite",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "sprint_id",
                table: "SprintInvite",
                newName: "SprintId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "SprintInvite",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "inviter_id",
                table: "SprintInvite",
                newName: "InviterId");

            migrationBuilder.RenameColumn(
                name: "invitee_id",
                table: "SprintInvite",
                newName: "InviteeId");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "SprintInvite",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_sprint_invite_sprint_id",
                table: "SprintInvite",
                newName: "IX_SprintInvite_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "SprintInvite",
                newName: "IX_SprintInvite_InviteeId");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "FirebaseToken",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "FirebaseToken",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "FirebaseToken",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "FirebaseToken",
                newName: "LastUpdated");

            migrationBuilder.RenameIndex(
                name: "IX_firebase_token_user_id",
                table: "FirebaseToken",
                newName: "IX_FirebaseToken_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AppDownloads",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "AppDownloads",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "device_platform",
                table: "AppDownloads",
                newName: "DevicePlatform");

            migrationBuilder.RenameColumn(
                name: "device_id",
                table: "AppDownloads",
                newName: "DeviceId");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "AppDownloads",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "AccessToken",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AccessToken",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "AccessToken",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "AccessToken",
                newName: "CreatedDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sprint",
                table: "Sprint",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Frineds",
                table: "Frineds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Achievement",
                table: "Achievement",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SprintParticipant",
                table: "SprintParticipant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SprintInvite",
                table: "SprintInvite",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SprintInvite_InviterId_InviteeId_SprintId",
                table: "SprintInvite",
                columns: new[] { "InviterId", "InviteeId", "SprintId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FirebaseToken",
                table: "FirebaseToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppDownloads",
                table: "AppDownloads",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessToken",
                table: "AccessToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievement_User_UserId",
                table: "Achievement",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FirebaseToken_User_UserId",
                table: "FirebaseToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Frineds_User_AcceptedUserId",
                table: "Frineds",
                column: "AcceptedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Frineds_User_SharedUserId",
                table: "Frineds",
                column: "SharedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Achievement_AchievementId1",
                table: "Notification",
                column: "AchievementId1",
                principalTable: "Achievement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_SprintInvite_SprintInviteId",
                table: "Notification",
                column: "SprintInviteId",
                principalTable: "SprintInvite",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sprint_User_CreatedById",
                table: "Sprint",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintInvite_User_InviteeId",
                table: "SprintInvite",
                column: "InviteeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintInvite_User_InviterId",
                table: "SprintInvite",
                column: "InviterId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintInvite_Sprint_SprintId",
                table: "SprintInvite",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintParticipant_Sprint_SprintId",
                table: "SprintParticipant",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintParticipant_User_UserId",
                table: "SprintParticipant",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_AccessToken_AccessTokenId",
                table: "User",
                column: "AccessTokenId",
                principalTable: "AccessToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_Notification_NotificationId",
                table: "UserNotification",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_User_ReceiverId",
                table: "UserNotification",
                column: "ReceiverId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_User_SenderId",
                table: "UserNotification",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
