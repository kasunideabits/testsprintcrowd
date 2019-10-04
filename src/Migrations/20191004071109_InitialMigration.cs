using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SprintCrowdBackEnd.Migrations
{
    public partial class InitialMigration : Migration
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
                name: "app_downloads",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    device_id = table.Column<string>(nullable: true),
                    device_platform = table.Column<string>(nullable: true),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_downloads", x => x.id);
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
                    code = table.Column<string>(nullable: true),
                    access_token_id = table.Column<int>(nullable: true),
                    language_preference = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    country_code = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
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
                name: "achievement",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievement", x => x.id);
                    table.ForeignKey(
                        name: "FK_achievement_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "firebase_token",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(nullable: true),
                    token = table.Column<string>(nullable: true),
                    last_updated = table.Column<DateTime>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "frineds",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(nullable: false),
                    friend_id = table.Column<int>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    generate_time = table.Column<DateTime>(nullable: false),
                    status_updated_time = table.Column<DateTime>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frineds", x => x.id);
                    table.ForeignKey(
                        name: "FK_frineds_user_friend_id",
                        column: x => x.friend_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_frineds_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    location = table.Column<string>(nullable: true),
                    number_of_participants = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    influencer_availability = table.Column<bool>(nullable: false),
                    influencer_email = table.Column<string>(nullable: true),
                    draft_event = table.Column<int>(nullable: false),
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
                    table.UniqueConstraint("AK_sprint_invite_inviter_id_invitee_id_sprint_id", x => new { x.inviter_id, x.invitee_id, x.sprint_id });
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
                    table.ForeignKey(
                        name: "FK_sprint_invite_sprint_sprint_id",
                        column: x => x.sprint_id,
                        principalTable: "sprint",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sprint_participant",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(nullable: false),
                    sprint_id = table.Column<int>(nullable: false),
                    stage = table.Column<int>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_participant", x => x.id);
                    table.ForeignKey(
                        name: "FK_sprint_participant_sprint_sprint_id",
                        column: x => x.sprint_id,
                        principalTable: "sprint",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprint_participant_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    notitication_type = table.Column<int>(nullable: false),
                    sender_id = table.Column<int>(nullable: false),
                    receiver_id = table.Column<int>(nullable: false),
                    sprint_invite_id = table.Column<int>(nullable: true),
                    achievement_id = table.Column<int>(nullable: true),
                    send_time = table.Column<DateTime>(nullable: false),
                    is_read = table.Column<bool>(nullable: false),
                    last_updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_achievement_achievement_id",
                        column: x => x.achievement_id,
                        principalTable: "achievement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.ForeignKey(
                        name: "FK_notification_sprint_invite_sprint_invite_id",
                        column: x => x.sprint_invite_id,
                        principalTable: "sprint_invite",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_achievement_user_id",
                table: "achievement",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_firebase_token_user_id",
                table: "firebase_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_frineds_friend_id",
                table: "frineds",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_frineds_user_id",
                table: "frineds",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_achievement_id",
                table: "notification",
                column: "achievement_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_receiver_id",
                table: "notification",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_sender_id",
                table: "notification",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_sprint_invite_id",
                table: "notification",
                column: "sprint_invite_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_created_by_id",
                table: "sprint",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_invitee_id",
                table: "sprint_invite",
                column: "invitee_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_invite_sprint_id",
                table: "sprint_invite",
                column: "sprint_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_participant_sprint_id",
                table: "sprint_participant",
                column: "sprint_id");

            migrationBuilder.CreateIndex(
                name: "IX_sprint_participant_user_id_sprint_id",
                table: "sprint_participant",
                columns: new[] { "user_id", "sprint_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_access_token_id",
                table: "user",
                column: "access_token_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_code",
                table: "user",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_downloads");

            migrationBuilder.DropTable(
                name: "firebase_token");

            migrationBuilder.DropTable(
                name: "frineds");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "sprint_participant");

            migrationBuilder.DropTable(
                name: "achievement");

            migrationBuilder.DropTable(
                name: "sprint_invite");

            migrationBuilder.DropTable(
                name: "sprint");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "access_token");
        }
    }
}
