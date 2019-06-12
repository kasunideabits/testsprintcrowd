using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class NumOfParticipantsForSprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number_of_participants",
                table: "sprint",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "number_of_participants",
                table: "sprint");
        }
    }
}
