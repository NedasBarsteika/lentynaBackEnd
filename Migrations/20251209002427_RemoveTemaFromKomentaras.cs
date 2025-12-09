using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTemaFromKomentaras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Komentarai_Temos_TemaId",
                table: "Komentarai");

            migrationBuilder.DropIndex(
                name: "IX_Komentarai_TemaId",
                table: "Komentarai");

            migrationBuilder.DropColumn(
                name: "TemaId",
                table: "Komentarai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TemaId",
                table: "Komentarai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Komentarai_TemaId",
                table: "Komentarai",
                column: "TemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Komentarai_Temos_TemaId",
                table: "Komentarai",
                column: "TemaId",
                principalTable: "Temos",
                principalColumn: "Id");
        }
    }
}
