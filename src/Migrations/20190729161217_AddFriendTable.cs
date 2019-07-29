using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddFriendTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "frineds",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(nullable: false),
                    friend_id = table.Column<int>(nullable: false),
                    code = table.Column<int>(nullable: false),
                    request_status = table.Column<int>(nullable: false),
                    send_time = table.Column<DateTime>(nullable: false),
                    status_updated_time = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frineds", x => x.id);
                    table.ForeignKey(
                        name: "FK_frineds_user_friend_id",
                        column: x => x.friend_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_frineds_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_frineds_friend_id",
                table: "frineds",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_frineds_user_id",
                table: "frineds",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "frineds");
        }
    }
}
