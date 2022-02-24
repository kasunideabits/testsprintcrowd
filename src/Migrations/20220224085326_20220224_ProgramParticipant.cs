using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _20220224_ProgramParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "program_participant",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    program_id = table.Column<int>(nullable: false),
                    stage = table.Column<int>(nullable: false),
                    sprint_program_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_participant", x => x.id);
                    table.ForeignKey(
                        name: "FK_program_participant_sprint_program_sprint_program_id",
                        column: x => x.sprint_program_id,
                        principalTable: "sprint_program",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_program_participant_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_program_participant_sprint_program_id",
                table: "program_participant",
                column: "sprint_program_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_participant_user_id",
                table: "program_participant",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "program_participant");
        }
    }
}
