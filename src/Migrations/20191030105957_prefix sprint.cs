using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class prefixsprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SprintNotification_Status",
                table: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "SprintNotificationType",
                table: "Notification",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SprintStatus",
                table: "Notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SprintNotificationType",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "SprintStatus",
                table: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "SprintNotification_Status",
                table: "Notification",
                nullable: true);
        }
    }
}
