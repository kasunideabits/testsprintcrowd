using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class AddUserPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_preferences",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    mon = table.Column<bool>(nullable: false),
                    tue = table.Column<bool>(nullable: false),
                    wed = table.Column<bool>(nullable: false),
                    thur = table.Column<bool>(nullable: false),
                    fri = table.Column<bool>(nullable: false),
                    sat = table.Column<bool>(nullable: false),
                    sun = table.Column<bool>(nullable: false),
                    morning = table.Column<bool>(nullable: false),
                    after_noon = table.Column<bool>(nullable: false),
                    evening = table.Column<bool>(nullable: false),
                    night = table.Column<bool>(nullable: false),
                    two_to_ten = table.Column<bool>(nullable: false),
                    ele_to_twenty = table.Column<bool>(nullable: false),
                    t_one_to_thirty = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_preferences", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_preferences_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_preferences_user_id",
                table: "user_preferences",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_preferences");
        }
    }
}
