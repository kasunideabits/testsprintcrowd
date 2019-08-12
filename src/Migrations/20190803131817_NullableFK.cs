using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class NullableFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_friend_id",
                table: "frineds");

            migrationBuilder.AlterColumn<int>(
                name: "friend_id",
                table: "frineds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_friend_id",
                table: "frineds",
                column: "friend_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_friend_id",
                table: "frineds");

            migrationBuilder.AlterColumn<int>(
                name: "friend_id",
                table: "frineds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_friend_id",
                table: "frineds",
                column: "friend_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
