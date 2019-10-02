using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddInfluencer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "influencer_availability",
                table: "sprint",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "influencer_email",
                table: "sprint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "influencer_availability",
                table: "sprint");

            migrationBuilder.DropColumn(
                name: "influencer_email",
                table: "sprint");
        }
    }
}
