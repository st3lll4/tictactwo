﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241202091151_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Domain.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConfigName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("InitialMoves")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxPieces")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovableGridSize")
                        .HasColumnType("INTEGER");

                    b.Property<char>("Player1Symbol")
                        .HasColumnType("TEXT");

                    b.Property<char>("Player2Symbol")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartingPlayer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WinningCondition")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BoardData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Config")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("GridCenterCol")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridCenterRow")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridStartCol")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridStartRow")
                        .HasColumnType("INTEGER");

                    b.Property<char>("MovingPlayer")
                        .HasColumnType("TEXT");

                    b.Property<int>("Player1PiecesPlaced")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Player2PiecesPlaced")
                        .HasColumnType("INTEGER");

                    b.Property<int>("User1Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("User2Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("User1Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PlayerType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Configuration", b =>
                {
                    b.HasOne("Domain.User", "User")
                        .WithMany("Configurations")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.HasOne("Domain.Configuration", "Configuration")
                        .WithMany("Games")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User1")
                        .WithMany("Games")
                        .HasForeignKey("User1Id")
                        .IsRequired();

                    b.HasOne("Domain.User", "User2")
                        .WithMany()
                        .HasForeignKey("User2Id");

                    b.Navigation("Configuration");

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("Domain.Configuration", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Navigation("Configurations");

                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}