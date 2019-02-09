using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class addLastUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessTokenId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "EventParticipant",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Event",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AccessToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserType = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessToken", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_AccessTokenId",
                table: "User",
                column: "AccessTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AccessToken_AccessTokenId",
                table: "User",
                column: "AccessTokenId",
                principalTable: "AccessToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AccessToken_AccessTokenId",
                table: "User");

            migrationBuilder.DropTable(
                name: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_User_AccessTokenId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AccessTokenId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Event");
        }
    }
}
