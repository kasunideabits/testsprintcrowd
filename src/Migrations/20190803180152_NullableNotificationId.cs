using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class NullableNotificationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_achievement_achievement_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_sprint_sprint_id",
                table: "notification");

            migrationBuilder.AlterColumn<int>(
                name: "sprint_id",
                table: "notification",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "achievement_id",
                table: "notification",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_notification_achievement_achievement_id",
                table: "notification",
                column: "achievement_id",
                principalTable: "achievement",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_sprint_sprint_id",
                table: "notification",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_achievement_achievement_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_sprint_sprint_id",
                table: "notification");

            migrationBuilder.AlterColumn<int>(
                name: "sprint_id",
                table: "notification",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "achievement_id",
                table: "notification",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_achievement_achievement_id",
                table: "notification",
                column: "achievement_id",
                principalTable: "achievement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_sprint_sprint_id",
                table: "notification",
                column: "sprint_id",
                principalTable: "sprint",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
