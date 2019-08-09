using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddLocationSprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lattitude",
                table: "sprint");

            migrationBuilder.DropColumn(
                name: "location_provided",
                table: "sprint");

            migrationBuilder.DropColumn(
                name: "longitutude",
                table: "sprint");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "sprint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "sprint");

            migrationBuilder.AddColumn<double>(
                name: "lattitude",
                table: "sprint",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "location_provided",
                table: "sprint",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "longitutude",
                table: "sprint",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
