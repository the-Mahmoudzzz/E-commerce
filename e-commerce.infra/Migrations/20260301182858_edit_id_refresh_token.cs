using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.infra.Migrations
{
    /// <inheritdoc />
    public partial class edit_id_refresh_token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId1",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserId1",
                table: "refreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "refreshTokens");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "refreshTokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId",
                table: "refreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens");
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "refreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserId1",
                table: "refreshTokens",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId1",
                table: "refreshTokens",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
