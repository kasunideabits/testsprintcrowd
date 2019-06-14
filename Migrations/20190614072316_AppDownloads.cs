using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowd.BackEnd.Migrations
{
    public partial class AppDownloads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "app_downloads",
                columns : table => new
                {
                    id = table.Column<int>(nullable: false).Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                        device_id = table.Column<int>(nullable : true),
                        device_platform = table.Column<string>(nullable : true)
                },

                constraints : table =>
                {
                    table.PrimaryKey("PK_app_downloads", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_download_id",
                table: "app_downloads",
                column: "id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_downloads"
            );

        }
    }
}