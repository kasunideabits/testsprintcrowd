using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddAlternateKeySprintInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_sprint_invite_inviter_id_invitee_id_sprint_id",
                table: "sprint_invite",
                columns: new[] { "inviter_id", "invitee_id", "sprint_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_sprint_invite_inviter_id_invitee_id_sprint_id",
                table: "sprint_invite");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite",
                column: "inviter_id");
        }
    }
}
