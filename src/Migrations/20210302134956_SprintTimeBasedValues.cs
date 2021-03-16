using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class SprintTimeBasedValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "duration_for_time_based_event",
                table: "sprint",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "is_time_based",
                table: "sprint",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration_for_time_based_event",
                table: "sprint");

            migrationBuilder.DropColumn(
                name: "is_time_based",
                table: "sprint");
        }
    }
}
