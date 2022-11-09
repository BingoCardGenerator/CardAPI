using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardApi.Migrations
{
    public partial class CreateCardTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BingoCardChallenges");

            migrationBuilder.DropTable(
                name: "BingoCards");

            migrationBuilder.CreateTable(
                name: "BingoCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BingoCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BingoCardChallenges",
                columns: table => new
                {
                    BingoCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BingoCardModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BingoCardChallenges", x => new { x.BingoCardId, x.ChallengeId });
                    table.ForeignKey(
                        name: "FK_BingoCardChallenges_BingoCards_BingoCardModelId",
                        column: x => x.BingoCardModelId,
                        principalTable: "BingoCards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BingoCardChallenges_BingoCardModelId",
                table: "BingoCardChallenges",
                column: "BingoCardModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BingoCardChallenges");

            migrationBuilder.DropTable(
                name: "BingoCards");
        }
    }
}
