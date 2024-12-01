using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class changes3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_UserId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Games",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Games_UserId",
                table: "Games",
                newName: "IX_Games_User1Id");

            migrationBuilder.AddColumn<int>(
                name: "User2Id",
                table: "Games",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_User2Id",
                table: "Games",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_User1Id",
                table: "Games",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_User2Id",
                table: "Games",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_User1Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_User2Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_User2Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User2Id",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "Games",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_User1Id",
                table: "Games",
                newName: "IX_Games_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_UserId",
                table: "Games",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
