using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class CorrectRelationForInviterInvitee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "sprint_invite");

            migrationBuilder.DropIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "sprint_invite",
                column: "invitee_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite",
                column: "inviter_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "sprint_invite");

            migrationBuilder.DropIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "sprint_invite",
                column: "invitee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite",
                column: "inviter_id",
                unique: true);
        }
    }
}
