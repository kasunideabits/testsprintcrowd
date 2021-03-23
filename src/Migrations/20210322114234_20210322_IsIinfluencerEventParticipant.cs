using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _20210322_IsIinfluencerEventParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_iinfluencer_event_participant",
                table: "sprint_participant",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_iinfluencer_event_participant",
                table: "sprint_participant");
        }
    }
}
