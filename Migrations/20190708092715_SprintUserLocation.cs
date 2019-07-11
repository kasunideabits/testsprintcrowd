using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class SprintUserLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "user",
                nullable : true,
                defaultValue : null
            );
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "user",
                nullable : true,
                defaultValue : null
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country",
                table: "user"
            );
            migrationBuilder.DropColumn(
                name: "city",
                table: "user"
            );

        }
    }
}