using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class RefactorRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "request_status",
                table: "frineds");

            migrationBuilder.RenameColumn(
                name: "send_time",
                table: "frineds",
                newName: "generate_time");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "frineds",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "generate_time",
                table: "frineds",
                newName: "send_time");

            migrationBuilder.AlterColumn<int>(
                name: "code",
                table: "frineds",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "request_status",
                table: "frineds",
                nullable: false,
                defaultValue: 0);
        }
    }
}
