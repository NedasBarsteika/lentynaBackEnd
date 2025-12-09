using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddBalsavimoKnygaJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalsavimoKnygos",
                columns: table => new
                {
                    BalsavimasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnygaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalsavimoKnygos", x => new { x.BalsavimasId, x.KnygaId });
                    table.ForeignKey(
                        name: "FK_BalsavimoKnygos_Balsavimai_BalsavimasId",
                        column: x => x.BalsavimasId,
                        principalTable: "Balsavimai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BalsavimoKnygos_Knygos_KnygaId",
                        column: x => x.KnygaId,
                        principalTable: "Knygos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BalsavimoKnygos_KnygaId",
                table: "BalsavimoKnygos",
                column: "KnygaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalsavimoKnygos");
        }
    }
}
