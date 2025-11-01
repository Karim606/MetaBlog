using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addrefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    revokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    replacedByTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    revokeReason = table.Column<int>(type: "int", nullable: true),
                    createdByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_RefreshTokens_replacedByTokenId",
                        column: x => x.replacedByTokenId,
                        principalTable: "RefreshTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_replacedByTokenId",
                table: "RefreshTokens",
                column: "replacedByTokenId",
                unique: true,
                filter: "[replacedByTokenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_userId",
                table: "RefreshTokens",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
