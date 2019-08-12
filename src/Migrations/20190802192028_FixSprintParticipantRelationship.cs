using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class FixSprintParticipantRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sprint_participant_sprint_sprint_id",
                table: "sprint_participant");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_participant_user_user_id",
                table: "sprint_participant");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "sprint_participant",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sprint_id",
                table: "sprint_participant",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sprint_participant_sprint_sprint_id",
                table: "sprint_participant");

            migrationBuilder.DropForeignKey(
                name: "FK_sprint_participant_user_user_id",
                table: "sprint_participant");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "sprint_participant",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "sprint_id",
                table: "sprint_participant",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_participant_sprint_sprint_id",
                table: "sprint_participant",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sprint_participant_user_user_id",
                table: "sprint_participant",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
