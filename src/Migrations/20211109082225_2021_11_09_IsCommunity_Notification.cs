using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _2021_11_09_IsCommunity_Notification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_community",
                table: "user_notification",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_community",
                table: "user_notification");
        }
    }
}
