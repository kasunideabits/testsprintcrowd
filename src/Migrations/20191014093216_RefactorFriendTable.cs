using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class RefactorFriendTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_friend_id",
                table: "frineds");

            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_user_id",
                table: "frineds");

            migrationBuilder.DropIndex(
                name: "IX_frineds_friend_id",
                table: "frineds");

            migrationBuilder.DropColumn(
                name: "code",
                table: "frineds");

            migrationBuilder.DropColumn(
                name: "friend_id",
                table: "frineds");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "frineds",
                newName: "shared_user_id");

            migrationBuilder.RenameColumn(
                name: "status_updated_time",
                table: "frineds",
                newName: "updated_time");

            migrationBuilder.RenameColumn(
                name: "generate_time",
                table: "frineds",
                newName: "created_time");

            migrationBuilder.RenameIndex(
                name: "IX_frineds_user_id",
                table: "frineds",
                newName: "IX_frineds_shared_user_id");

            migrationBuilder.AddColumn<int>(
                name: "accepted_user_id",
                table: "frineds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_frineds_accepted_user_id",
                table: "frineds",
                column: "accepted_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_accepted_user_id",
                table: "frineds",
                column: "accepted_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_shared_user_id",
                table: "frineds",
                column: "shared_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_accepted_user_id",
                table: "frineds");

            migrationBuilder.DropForeignKey(
                name: "FK_frineds_user_shared_user_id",
                table: "frineds");

            migrationBuilder.DropIndex(
                name: "IX_frineds_accepted_user_id",
                table: "frineds");

            migrationBuilder.DropColumn(
                name: "accepted_user_id",
                table: "frineds");

            migrationBuilder.RenameColumn(
                name: "updated_time",
                table: "frineds",
                newName: "status_updated_time");

            migrationBuilder.RenameColumn(
                name: "shared_user_id",
                table: "frineds",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "frineds",
                newName: "generate_time");

            migrationBuilder.RenameIndex(
                name: "IX_frineds_shared_user_id",
                table: "frineds",
                newName: "IX_frineds_user_id");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "frineds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "friend_id",
                table: "frineds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_frineds_friend_id",
                table: "frineds",
                column: "friend_id");

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_friend_id",
                table: "frineds",
                column: "friend_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_frineds_user_user_id",
                table: "frineds",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
