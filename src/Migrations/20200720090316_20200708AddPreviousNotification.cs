using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _20200708AddPreviousNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "previous_distance",
                table: "notification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "previous_sprint_name",
                table: "notification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "previous_start_date",
                table: "notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "previous_distance",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "previous_sprint_name",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "previous_start_date",
                table: "notification");
        }
    }
}
