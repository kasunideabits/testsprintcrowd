using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class preferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "two_to_ten",
                table: "user_preferences",
                newName: "two_to_five");

            migrationBuilder.RenameColumn(
                name: "ele_to_twenty",
                table: "user_preferences",
                newName: "thirty_one_to_forty_one");

            migrationBuilder.AddColumn<bool>(
                name: "eleven_to_fifteen",
                table: "user_preferences",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "six_to_ten",
                table: "user_preferences",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "sixteen_to_twenty",
                table: "user_preferences",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eleven_to_fifteen",
                table: "user_preferences");

            migrationBuilder.DropColumn(
                name: "six_to_ten",
                table: "user_preferences");

            migrationBuilder.DropColumn(
                name: "sixteen_to_twenty",
                table: "user_preferences");

            migrationBuilder.RenameColumn(
                name: "two_to_five",
                table: "user_preferences",
                newName: "two_to_ten");

            migrationBuilder.RenameColumn(
                name: "thirty_one_to_forty_one",
                table: "user_preferences",
                newName: "ele_to_twenty");
        }
    }
}
