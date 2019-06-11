﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowd.BackEnd.Migrations
{
  public partial class Initial : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "access_token",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            token = table.Column<string>(nullable: true),
            last_updated = table.Column<DateTime>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_access_token", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "user",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            user_type = table.Column<int>(nullable: false),
            facebook_user_id = table.Column<string>(nullable: true),
            email = table.Column<string>(nullable: true),
            name = table.Column<string>(nullable: true),
            profile_picture = table.Column<string>(nullable: true),
            access_token_id = table.Column<int>(nullable: true),
            last_updated = table.Column<DateTime>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_user", x => x.id);
            table.ForeignKey(
                      name: "FK_user_access_token_access_token_id",
                      column: x => x.access_token_id,
                      principalTable: "access_token",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "sprint",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            distance = table.Column<int>(nullable: false),
            created_by_id = table.Column<int>(nullable: true),
            start_date_time = table.Column<DateTime>(nullable: false),
            type = table.Column<int>(nullable: false),
            status = table.Column<int>(nullable: false),
            location_provided = table.Column<bool>(nullable: false),
            lattitude = table.Column<double>(nullable: false),
            longitutude = table.Column<double>(nullable: false),
            last_updated = table.Column<DateTime>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_sprint", x => x.id);
            table.ForeignKey(
                      name: "FK_sprint_user_created_by_id",
                      column: x => x.created_by_id,
                      principalTable: "user",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "sprint_participant",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            user_id = table.Column<int>(nullable: true),
            stage = table.Column<int>(nullable: false),
            last_updated = table.Column<DateTime>(nullable: false),
            sprint_id = table.Column<int>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_sprint_participant", x => x.id);
            table.ForeignKey(
                      name: "FK_sprint_participant_sprint_sprint_id",
                      column: x => x.sprint_id,
                      principalTable: "sprint",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_sprint_participant_user_user_id",
                      column: x => x.user_id,
                      principalTable: "user",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_sprint_created_by_id",
          table: "sprint",
          column: "created_by_id");

      migrationBuilder.CreateIndex(
          name: "IX_sprint_participant_sprint_id",
          table: "sprint_participant",
          column: "sprint_id");

      migrationBuilder.CreateIndex(
          name: "IX_sprint_participant_user_id",
          table: "sprint_participant",
          column: "user_id");

      migrationBuilder.CreateIndex(
          name: "IX_user_access_token_id",
          table: "user",
          column: "access_token_id");

      migrationBuilder.CreateIndex(
          name: "IX_user_email",
          table: "user",
          column: "email",
          unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "sprint_participant");

      migrationBuilder.DropTable(
          name: "sprint");

      migrationBuilder.DropTable(
          name: "user");

      migrationBuilder.DropTable(
          name: "access_token");
    }
  }
}
