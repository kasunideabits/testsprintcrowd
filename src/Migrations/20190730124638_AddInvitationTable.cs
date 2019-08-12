using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddInvitationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sprint_invite",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    sprint_id = table.Column<int>(nullable: false),
                    inviter_id = table.Column<int>(nullable: false),
                    invitee_id = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_invite", x => x.id);
                    table.ForeignKey(
                        name: "FK_sprint_invite_user_invitee_id",
                        column: x => x.invitee_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprint_invite_user_inviter_id",
                        column: x => x.inviter_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "sprint_invite",
                column: "invitee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_inviter_id",
                table: "sprint_invite",
                column: "inviter_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sprint_invite");
        }
    }
}
