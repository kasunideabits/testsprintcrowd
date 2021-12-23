using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class _20211222_SprintProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sprint_program",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    duration = table.Column<int>(nullable: false),
                    is_private = table.Column<bool>(nullable: false),
                    image_url = table.Column<string>(nullable: true),
                    get_social_link = table.Column<string>(nullable: true),
                    program_code = table.Column<string>(nullable: true),
                    start_date = table.Column<DateTime>(nullable: false),
                    created_by_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_program", x => x.id);
                    table.ForeignKey(
                        name: "FK_sprint_program_user_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sprint_program_created_by_id",
                table: "sprint_program",
                column: "created_by_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sprint_program");
        }
    }
}
