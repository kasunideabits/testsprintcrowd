using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class ChangeAchievementNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_achievement_achievement_id1",
                table: "notification");

            migrationBuilder.DropIndex(
                name: "IX_notification_achievement_id1",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "achievement_id1",
                table: "notification");

            migrationBuilder.AddColumn<int>(
                name: "achievement_type",
                table: "notification",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_achievement_id",
                table: "notification",
                column: "achievement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_achievement_achievement_id",
                table: "notification",
                column: "achievement_id",
                principalTable: "achievement",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_achievement_achievement_id",
                table: "notification");

            migrationBuilder.DropIndex(
                name: "IX_notification_achievement_id",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "achievement_type",
                table: "notification");

            migrationBuilder.AddColumn<int>(
                name: "achievement_id1",
                table: "notification",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_achievement_id1",
                table: "notification",
                column: "achievement_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_achievement_achievement_id1",
                table: "notification",
                column: "achievement_id1",
                principalTable: "achievement",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
