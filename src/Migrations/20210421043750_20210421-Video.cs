using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _20210421Video : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "video_link",
                table: "sprint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "video_type",
                table: "sprint",
                type: "varchar(20)",
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "video_link",
                table: "sprint");

            migrationBuilder.DropColumn(
                name: "video_type",
                table: "sprint");
        }
    }
}
