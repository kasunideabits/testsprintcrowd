﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SprintCrowd.BackEnd.Infrastructure.Persistence;

namespace SprintCrowd.BackEnd.Migrations
{
    [DbContext(typeof(ScrowdDbContext))]
    [Migration("20190708092715_SprintUserLocation")]
    partial class SprintUserLocation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.AccessToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("LastUpdated")
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

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnName("last_updated");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<int?>("UserId")
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

                    b.Property<string>("DeviceId")
                        .HasColumnName("device_id");

                    b.Property<string>("DevicePlatform")
                        .HasColumnName("device_platform");

                    b.Property<DateTime>("LastUpdated")
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

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnName("last_updated");

                    b.Property<double>("Lattitude")
                        .HasColumnName("lattitude");

                    b.Property<bool>("LocationProvided")
                        .HasColumnName("location_provided");

                    b.Property<double>("Longitutude")
                        .HasColumnName("longitutude");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<int>("NumberOfParticipants")
                        .HasColumnName("number_of_participants");

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

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnName("last_updated");

                    b.Property<int?>("SprintId")
                        .HasColumnName("sprint_id");

                    b.Property<int>("Stage")
                        .HasColumnName("stage");

                    b.Property<int?>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SprintId");

                    b.HasIndex("UserId");

                    b.ToTable("sprint_participant");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("AccessTokenId")
                        .HasColumnName("access_token_id");

                    b.Property<string>("Email")
                        .HasColumnName("email");

                    b.Property<string>("FacebookUserId")
                        .HasColumnName("facebook_user_id");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnName("last_updated");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("ProfilePicture")
                        .HasColumnName("profile_picture");

                    b.Property<int>("UserType")
                        .HasColumnName("user_type");

                    b.HasKey("Id");

                    b.HasIndex("AccessTokenId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Achievement", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.FirebaseMessagingToken", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "CreatedBy")
                        .WithMany("Sprint")
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.SprintParticipant", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.Sprint")
                        .WithMany("Participants")
                        .HasForeignKey("SprintId");

                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.HasOne("SprintCrowd.BackEnd.Infrastructure.Persistence.Entities.AccessToken", "AccessToken")
                        .WithMany()
                        .HasForeignKey("AccessTokenId");
                });
#pragma warning restore 612, 618
        }
    }
}
