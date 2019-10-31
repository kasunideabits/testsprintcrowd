using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class NotificaitonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_user_sender_id",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "is_read",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "notitication_type",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "send_time",
                table: "notification");

            migrationBuilder.AlterColumn<int>(
                name: "sender_id",
                table: "notification",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "accepter_id",
                table: "notification",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "requester_id",
                table: "notification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "notification",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "notification",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sprint_id",
                table: "notification",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "updator_id",
                table: "notification",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "sprint_base",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                    distance = table.Column<int>(nullable: false),
                    start_date_time = table.Column<DateTime>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    number_of_participants = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_base", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_sprint_id",
                table: "notification",
                column: "sprint_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_user_sender_id",
                table: "notification",
                column: "sender_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_sprint_base_sprint_id",
                table: "notification",
                column: "sprint_id",
                principalTable: "sprint_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_user_sender_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_sprint_base_sprint_id",
                table: "notification");

            migrationBuilder.DropTable(
                name: "sprint_base");

            migrationBuilder.DropIndex(
                name: "IX_notification_sprint_id",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "accepter_id",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "requester_id",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "status",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "type",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "sprint_id",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "updator_id",
                table: "notification");

            migrationBuilder.AlterColumn<int>(
                name: "sender_id",
                table: "notification",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_read",
                table: "notification",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "notitication_type",
                table: "notification",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "send_time",
                table: "notification",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_notification_user_sender_id",
                table: "notification",
                column: "sender_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
