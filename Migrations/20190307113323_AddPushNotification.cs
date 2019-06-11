﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowd.BackEnd.Migrations
{
  public partial class AddPushNotification : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "firebase_token",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            user_id = table.Column<int>(nullable: true),
            token = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_firebase_token", x => x.id);
            table.ForeignKey(
                      name: "FK_firebase_token_user_user_id",
                      column: x => x.user_id,
                      principalTable: "user",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_firebase_token_user_id",
          table: "firebase_token",
          column: "user_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "firebase_token");
    }
  }
}
