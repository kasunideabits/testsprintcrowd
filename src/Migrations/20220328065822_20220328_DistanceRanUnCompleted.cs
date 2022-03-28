using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _20220328_DistanceRanUnCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "distance_ran_un_completed",
                table: "sprint_participant",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "distance_ran_un_completed",
                table: "sprint_participant");
        }
    }
}
