using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AddNotificationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    notitication_type = table.Column<int>(nullable: false),
                    sender_id = table.Column<int>(nullable: false),
                    receiver_id = table.Column<int>(nullable: false),
                    send_time = table.Column<DateTime>(nullable: false),
                    is_read = table.Column<bool>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_user_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notification_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sprint_invitation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    sprint_id = table.Column<int>(nullable: false),
                    inviter_id = table.Column<int>(nullable: false),
                    invitee_id = table.Column<int>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_invitation", x => x.id);
                    table.ForeignKey(
                        name: "FK_sprint_invitation_user_invitee_id",
                        column: x => x.invitee_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprint_invitation_user_inviter_id",
                        column: x => x.inviter_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprint_invitation_sprint_sprint_id",
                        column: x => x.sprint_id,
                        principalTable: "sprint",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sprint_invitation_notification",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    sprint_id = table.Column<int>(nullable: false),
                    notification_id = table.Column<int>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_invitation_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_sprint_invitation_notification_notification_notification_id",
                        column: x => x.notification_id,
                        principalTable: "notification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprint_invitation_notification_sprint_sprint_id",
                        column: x => x.sprint_id,
                        principalTable: "sprint",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_receiver_id",
                table: "notification",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_sender_id",
                table: "notification",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invitation_invitee_id",
                table: "sprint_invitation",
                column: "invitee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invitation_inviter_id",
                table: "sprint_invitation",
                column: "inviter_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invitation_sprint_id",
                table: "sprint_invitation",
                column: "sprint_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invitation_notification_notification_id",
                table: "sprint_invitation_notification",
                column: "notification_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invitation_notification_sprint_id",
                table: "sprint_invitation_notification",
                column: "sprint_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sprint_invitation");

            migrationBuilder.DropTable(
                name: "sprint_invitation_notification");

            migrationBuilder.DropTable(
                name: "notification");
        }
    }
}
