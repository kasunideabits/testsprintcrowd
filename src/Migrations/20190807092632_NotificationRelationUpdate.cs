using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class NotificationRelationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_sprint_sprint_id",
                table: "notification");

            migrationBuilder.RenameColumn(
                name: "sprint_id",
                table: "notification",
                newName: "sprint_invite_id");

            migrationBuilder.RenameIndex(
                name: "IX_notification_sprint_id",
                table: "notification",
                newName: "IX_notification_sprint_invite_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_sprint_id",
                table: "sprint_invite",
                column: "sprint_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_sprint_invite_sprint_invite_id",
                table: "notification",
                column: "sprint_invite_id",
                principalTable: "sprint_invite",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_invite_sprint_sprint_id",
                table: "sprint_invite",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_sprint_invite_sprint_invite_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_invite_sprint_sprint_id",
                table: "sprint_invite");

            migrationBuilder.DropIndex(
                name: "IX_sprint_invite_sprint_id",
                table: "sprint_invite");

            migrationBuilder.RenameColumn(
                name: "sprint_invite_id",
                table: "notification",
                newName: "sprint_id");

            migrationBuilder.RenameIndex(
                name: "IX_notification_sprint_invite_id",
                table: "notification",
                newName: "IX_notification_sprint_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_sprint_sprint_id",
                table: "notification",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
