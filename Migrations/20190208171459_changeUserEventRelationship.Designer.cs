﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SprintCrowdBackEnd.Infrastructure.Persistence;

namespace SprintCrowdBackEnd.Migrations
{
    [DbContext(typeof(ScrowdDbContext))]
    [Migration("20190208171459_changeUserEventRelationship")]
    partial class changeUserEventRelationship
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedById");

                    b.Property<int>("Distance");

                    b.Property<double>("Lattitude");

                    b.Property<bool>("LocationProvided");

                    b.Property<double>("Longitutude");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDateTime");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.EventParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Stage");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("EventParticipant");
                });

            modelBuilder.Entity("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FacebookUserId");

                    b.Property<string>("Name");

                    b.Property<string>("ProfilePicture");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.Event", b =>
                {
                    b.HasOne("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.User", "CreatedBy")
                        .WithMany("Events")
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.EventParticipant", b =>
                {
                    b.HasOne("SprintCrowdBackEnd.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
