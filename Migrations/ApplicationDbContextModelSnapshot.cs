﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SprintCrowdBackEnd.Models;

namespace SprintCrowdBackEnd.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SprintCrowdBackEnd.Models.ProfilePicture", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("Height");

                    b.Property<string>("Url");

                    b.Property<int>("Width");

                    b.HasKey("UserId");

                    b.ToTable("ProfilePictures");
                });

            modelBuilder.Entity("SprintCrowdBackEnd.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FbUserId");

                    b.Property<string>("FirstName");

                    b.Property<DateTime>("LastLoggedInTime");

                    b.Property<string>("LastName");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SprintCrowdBackEnd.Models.ProfilePicture", b =>
                {
                    b.HasOne("SprintCrowdBackEnd.Models.User", "User")
                        .WithOne("ProfilePicture")
                        .HasForeignKey("SprintCrowdBackEnd.Models.ProfilePicture", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
