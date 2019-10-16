using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class UserColorCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color_code",
                table: "user",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color_code",
                table: "user");
        }
    }
}
