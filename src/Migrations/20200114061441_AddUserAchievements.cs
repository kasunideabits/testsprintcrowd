using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class AddUserAchievements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_achivements",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    achived_on = table.Column<DateTime>(nullable: false),
                    percentage = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_achivements", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_achivements_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_achivements_user_id",
                table: "user_achivements",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_achivements");
        }
    }
}
