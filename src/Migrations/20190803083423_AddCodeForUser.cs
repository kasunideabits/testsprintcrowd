using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddCodeForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "user",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_code",
                table: "user",
                column: "code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_code",
                table: "user");

            migrationBuilder.DropColumn(
                name: "code",
                table: "user");
        }
    }
}
