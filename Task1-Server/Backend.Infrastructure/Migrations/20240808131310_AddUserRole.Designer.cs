﻿// <auto-generated />
using System;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240808131310_AddUserRole")]
    partial class AddUserRole
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Infrastructure.Models.Content", b =>
                {
                    b.Property<int>("ContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ContentId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ContentId");

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.FillingGroup", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("FocusId")
                        .HasColumnType("integer");

                    b.Property<int>("AddGrId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "FocusId");

                    b.HasIndex("FocusId");

                    b.ToTable("FillingGroups");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.FocusGroup", b =>
                {
                    b.Property<int>("FocGrId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FocGrId"));

                    b.Property<string>("Age")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Race")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FocGrId");

                    b.ToTable("FocusGroups");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Reaction", b =>
                {
                    b.Property<int>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ReactionId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ContentId")
                        .HasColumnType("integer");

                    b.Property<int>("Grade")
                        .HasColumnType("integer");

                    b.Property<int>("InterestRate")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ReactionId");

                    b.HasIndex("ContentId");

                    b.HasIndex("UserId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Result", b =>
                {
                    b.Property<int>("ResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ResultId"));

                    b.Property<string>("AvrEmotion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.HasKey("ResultId");

                    b.HasIndex("SessionId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SessionId"));

                    b.Property<int>("ContentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FocGrId")
                        .HasColumnType("integer");

                    b.Property<int>("FocusGroupFocGrId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("SessionId");

                    b.HasIndex("ContentId");

                    b.HasIndex("FocusGroupFocGrId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.FillingGroup", b =>
                {
                    b.HasOne("Backend.Infrastructure.Models.FocusGroup", "FocusGroup")
                        .WithMany("FillingGroups")
                        .HasForeignKey("FocusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Infrastructure.Models.User", "User")
                        .WithMany("FillingGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FocusGroup");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Reaction", b =>
                {
                    b.HasOne("Backend.Infrastructure.Models.Content", "Content")
                        .WithMany("Reactions")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Infrastructure.Models.User", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Content");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Result", b =>
                {
                    b.HasOne("Backend.Infrastructure.Models.Session", "Session")
                        .WithMany("Results")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Session", b =>
                {
                    b.HasOne("Backend.Infrastructure.Models.Content", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Infrastructure.Models.FocusGroup", "FocusGroup")
                        .WithMany("Sessions")
                        .HasForeignKey("FocusGroupFocGrId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Content");

                    b.Navigation("FocusGroup");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Content", b =>
                {
                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.FocusGroup", b =>
                {
                    b.Navigation("FillingGroups");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.Session", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("Backend.Infrastructure.Models.User", b =>
                {
                    b.Navigation("FillingGroups");

                    b.Navigation("Reactions");
                });
#pragma warning restore 612, 618
        }
    }
}
