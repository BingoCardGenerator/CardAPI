using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardApi.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BingoCardChallenges_BingoCards_BingoCardModelId",
                table: "BingoCardChallenges");

            migrationBuilder.DropIndex(
                name: "IX_BingoCardChallenges_BingoCardModelId",
                table: "BingoCardChallenges");

            migrationBuilder.DropColumn(
                name: "BingoCardModelId",
                table: "BingoCardChallenges");

            migrationBuilder.AddForeignKey(
                name: "FK_BingoCardChallenges_BingoCards_BingoCardId",
                table: "BingoCardChallenges",
                column: "BingoCardId",
                principalTable: "BingoCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BingoCardChallenges_BingoCards_BingoCardId",
                table: "BingoCardChallenges");

            migrationBuilder.AddColumn<Guid>(
                name: "BingoCardModelId",
                table: "BingoCardChallenges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BingoCardChallenges_BingoCardModelId",
                table: "BingoCardChallenges",
                column: "BingoCardModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_BingoCardChallenges_BingoCards_BingoCardModelId",
                table: "BingoCardChallenges",
                column: "BingoCardModelId",
                principalTable: "BingoCards",
                principalColumn: "Id");
        }
    }
}
