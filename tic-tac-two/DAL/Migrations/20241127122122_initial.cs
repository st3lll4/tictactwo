﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Player1Symbol = table.Column<char>(type: "TEXT", nullable: false),
                    Player2Symbol = table.Column<char>(type: "TEXT", nullable: false),
                    StartingPlayer = table.Column<string>(type: "TEXT", nullable: false),
                    MovableGridSize = table.Column<int>(type: "INTEGER", nullable: false),
                    WinningCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    InitialMoves = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxPieces = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configurations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    BoardData = table.Column<string>(type: "TEXT", nullable: false),
                    MovingPlayer = table.Column<char>(type: "TEXT", nullable: false),
                    Config = table.Column<string>(type: "TEXT", nullable: false),
                    Player1PiecesPlaced = table.Column<int>(type: "INTEGER", nullable: false),
                    Player2PiecesPlaced = table.Column<int>(type: "INTEGER", nullable: false),
                    GridStartRow = table.Column<int>(type: "INTEGER", nullable: false),
                    GridStartCol = table.Column<int>(type: "INTEGER", nullable: false),
                    GridCenterRow = table.Column<int>(type: "INTEGER", nullable: false),
                    GridCenterCol = table.Column<int>(type: "INTEGER", nullable: false),
                    GridSize = table.Column<int>(type: "INTEGER", nullable: false),
                    WinCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    IsStandardTicTacToe = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConfigurationId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_UserId",
                table: "Configurations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ConfigurationId",
                table: "Games",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                table: "Games",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
