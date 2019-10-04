using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddUniqueIndexSprintParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sprint_participant_user_id",
                table: "sprint_participant");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_participant_user_id_sprint_id",
                table: "sprint_participant",
                columns: new[] { "user_id", "sprint_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sprint_participant_user_id_sprint_id",
                table: "sprint_participant");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_participant_user_id",
                table: "sprint_participant",
                column: "user_id");
        }
    }
}
