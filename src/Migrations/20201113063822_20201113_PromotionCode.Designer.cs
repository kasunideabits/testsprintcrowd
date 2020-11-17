﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowdBackEnd.Migrations
{
    [DbContext(typeof(ScrowdDbContext))]
    [Migration("20201113063822_20201113_PromotionCode")]
    partial class _20201113_PromotionCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.AccessToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<string>("Token")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.ToTable("access_token");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Achievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("achievement");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.AppDownloads", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<string>("DeviceId")
                        .HasColumnName("device_id");

                    b.Property<string>("DevicePlatform")
                        .HasColumnName("device_platform");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.HasKey("Id");

                    b.ToTable("app_downloads");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.FirebaseMessagingToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnName("last_updated");

                    b.Property<string>("Token")
                        .HasColumnName("token");

                    b.Property<int?>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("firebase_token");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("AcceptedUserId")
                        .HasColumnName("accepted_user_id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("SharedUserId")
                        .HasColumnName("shared_user_id");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnName("updated_time");

                    b.HasKey("Id");

                    b.HasIndex("AcceptedUserId");

                    b.HasIndex("SharedUserId");

                    b.ToTable("frineds");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("AchievementId")
                        .HasColumnName("achievement_id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int?>("SprintInviteId")
                        .HasColumnName("sprint_invite_id");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.HasIndex("SprintInviteId");

                    b.ToTable("notification");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.PromoCodeUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<string>("PromoCode")
                        .HasColumnName("promo_code");

                    b.Property<int>("SprintId")
                        .HasColumnName("sprint_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("promo_code_user");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("CreatedById")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<int>("Distance")
                        .HasColumnName("distance");

                    b.Property<int>("DraftEvent")
                        .HasColumnName("draft_event");

                    b.Property<string>("ImageUrl")
                        .HasColumnName("image_url");

                    b.Property<bool>("InfluencerAvailability")
                        .HasColumnName("influencer_availability");

                    b.Property<string>("InfluencerEmail")
                        .HasColumnName("influencer_email");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<string>("Location")
                        .HasColumnName("location");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<int>("NumberOfParticipants")
                        .HasColumnName("number_of_participants");

                    b.Property<string>("PromotionCode")
                        .HasColumnName("promotion_code");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnName("start_date_time");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("sprint");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintInvite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<int>("InviteeId")
                        .HasColumnName("invitee_id");

                    b.Property<int>("InviterId")
                        .HasColumnName("inviter_id");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("SprintId")
                        .HasColumnName("sprint_id");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasAlternateKey("InviterId", "InviteeId", "SprintId");

                    b.HasIndex("InviteeId");

                    b.HasIndex("SprintId");

                    b.ToTable("sprint_invite");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<int>("DistanceRan")
                        .HasColumnName("distance_ran");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnName("finish_time");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("SprintId")
                        .HasColumnName("sprint_id");

                    b.Property<int>("Stage")
                        .HasColumnName("stage");

                    b.Property<DateTime>("StartedTime")
                        .HasColumnName("started_time");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SprintId");

                    b.HasIndex("UserId", "SprintId")
                        .IsUnique();

                    b.ToTable("sprint_participant");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("AccessTokenId")
                        .HasColumnName("access_token_id");

                    b.Property<string>("City")
                        .HasColumnName("city");

                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("code");

                    b.Property<string>("ColorCode")
                        .HasColumnName("color_code");

                    b.Property<string>("Country")
                        .HasColumnName("country");

                    b.Property<string>("CountryCode")
                        .HasColumnName("country_code");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<string>("Email")
                        .HasColumnName("email");

                    b.Property<string>("FacebookUserId")
                        .HasColumnName("facebook_user_id");

                    b.Property<string>("LanguagePreference")
                        .HasColumnName("language_preference");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("ProfilePicture")
                        .HasColumnName("profile_picture");

                    b.Property<int>("UserState")
                        .HasColumnName("user_state");

                    b.Property<int>("UserType")
                        .HasColumnName("user_type");

                    b.HasKey("Id");

                    b.HasIndex("AccessTokenId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserAchievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("AchivedOn")
                        .HasColumnName("achived_on");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("Percentage")
                        .HasColumnName("percentage");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_achivements");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_activity");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("BadgeValue")
                        .HasColumnName("badge_value");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<int>("NotificationId")
                        .HasColumnName("notification_id");

                    b.Property<int>("ReceiverId")
                        .HasColumnName("receiver_id");

                    b.Property<int?>("SenderId")
                        .HasColumnName("sender_id");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("user_notification");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserNotificationReminder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<bool>("EventStart")
                        .HasColumnName("event_start");

                    b.Property<bool>("FiftyM")
                        .HasColumnName("fifty_m");

                    b.Property<bool>("FinalCall")
                        .HasColumnName("final_call");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<bool>("OneH")
                        .HasColumnName("one_h");

                    b.Property<bool>("TwentyFourH")
                        .HasColumnName("twenty_four_h");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_notification_reminders");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("AfterNoon")
                        .HasColumnName("after_noon");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_date");

                    b.Property<bool>("ElevenToFifteen")
                        .HasColumnName("eleven_to_fifteen");

                    b.Property<bool>("Evening")
                        .HasColumnName("evening");

                    b.Property<bool>("Fri")
                        .HasColumnName("fri");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("last_updated");

                    b.Property<bool>("Mon")
                        .HasColumnName("mon");

                    b.Property<bool>("Morning")
                        .HasColumnName("morning");

                    b.Property<bool>("Night")
                        .HasColumnName("night");

                    b.Property<bool>("Sat")
                        .HasColumnName("sat");

                    b.Property<bool>("SixToTen")
                        .HasColumnName("six_to_ten");

                    b.Property<bool>("SixteenToTwenty")
                        .HasColumnName("sixteen_to_twenty");

                    b.Property<bool>("Sun")
                        .HasColumnName("sun");

                    b.Property<bool>("TOneToThirty")
                        .HasColumnName("t_one_to_thirty");

                    b.Property<bool>("ThirtyOneToFortyOne")
                        .HasColumnName("thirty_one_to_forty_one");

                    b.Property<bool>("Thur")
                        .HasColumnName("thur");

                    b.Property<bool>("Tue")
                        .HasColumnName("tue");

                    b.Property<bool>("TwoToFive")
                        .HasColumnName("two_to_five");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.Property<bool>("Wed")
                        .HasColumnName("wed");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_preferences");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.AchievementNoticiation", b =>
                {
                    b.HasBaseType("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Notification");

                    b.Property<DateTime>("AchievedOn")
                        .HasColumnName("achieved_on");

                    b.Property<int>("AchievementType")
                        .HasColumnName("achievement_type");

                    b.ToTable("notification");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.FriendNoticiation", b =>
                {
                    b.HasBaseType("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Notification");

                    b.Property<int?>("AccepterId")
                        .HasColumnName("accepter_id");

                    b.Property<int?>("RequesterId")
                        .HasColumnName("requester_id");

                    b.Property<string>("Status")
                        .HasColumnName("status");

                    b.ToTable("notification");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintNotification", b =>
                {
                    b.HasBaseType("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Notification");

                    b.Property<int>("Distance")
                        .HasColumnName("distance");

                    b.Property<int>("NumberOfParticipants")
                        .HasColumnName("number_of_participants");

                    b.Property<int>("SprintId")
                        .HasColumnName("sprint_id");

                    b.Property<string>("SprintName")
                        .HasColumnName("sprint_name");

                    b.Property<int>("SprintNotificationType")
                        .HasColumnName("sprint_notification_type");

                    b.Property<int>("SprintStatus")
                        .HasColumnName("sprint_status");

                    b.Property<int>("SprintType")
                        .HasColumnName("sprint_type");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnName("start_date_time");

                    b.Property<int?>("UpdatorId")
                        .HasColumnName("updator_id");

                    b.ToTable("notification");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Achievement", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("Achievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.FirebaseMessagingToken", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Friend", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "AcceptedUser")
                        .WithMany("friendsAccepted")
                        .HasForeignKey("AcceptedUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "SharedUser")
                        .WithMany("friendsShared")
                        .HasForeignKey("SharedUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Notification", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Achievement")
                        .WithMany("Notificatoins")
                        .HasForeignKey("AchievementId");

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintInvite")
                        .WithMany("Notification")
                        .HasForeignKey("SprintInviteId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "CreatedBy")
                        .WithMany("Sprint")
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintInvite", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "Invitee")
                        .WithMany("Invitee")
                        .HasForeignKey("InviteeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "Inviter")
                        .WithMany("Inviter")
                        .HasForeignKey("InviterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint", "Sprint")
                        .WithMany("SprintInvites")
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintParticipant", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint", "Sprint")
                        .WithMany("Participants")
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("Participates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.AccessToken", "AccessToken")
                        .WithMany()
                        .HasForeignKey("AccessTokenId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserAchievement", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserActivity", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("UserActivity")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserNotification", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Notification", "Notification")
                        .WithMany("UserNotification")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "Receiver")
                        .WithMany("ReceiverNotification")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "Sender")
                        .WithMany("SenderNotification")
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserNotificationReminder", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithOne("UserNotificationReminder")
                        .HasForeignKey("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserNotificationReminder", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserPreference", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithOne("UserPreference")
                        .HasForeignKey("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.UserPreference", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
