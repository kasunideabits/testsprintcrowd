using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class UserNotificationReminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_notification_reminders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    twenty_four_h = table.Column<bool>(nullable: false),
                    one_h = table.Column<bool>(nullable: false),
                    fifty_m = table.Column<bool>(nullable: false),
                    event_start = table.Column<bool>(nullable: false),
                    final_call = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_notification_reminders", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_notification_reminders_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_notification_reminders_user_id",
                table: "user_notification_reminders",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_notification_reminders");
        }
    }
}
