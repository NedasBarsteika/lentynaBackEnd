using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceNuotaikaJsonWithJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZanrasIds",
                table: "Nuotaikos");

            migrationBuilder.CreateTable(
                name: "NuotaikosZanrai",
                columns: table => new
                {
                    NuotaikaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ZanrasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NuotaikosZanrai", x => new { x.NuotaikaId, x.ZanrasId });
                    table.ForeignKey(
                        name: "FK_NuotaikosZanrai_Nuotaikos_NuotaikaId",
                        column: x => x.NuotaikaId,
                        principalTable: "Nuotaikos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NuotaikosZanrai_Zanrai_ZanrasId",
                        column: x => x.ZanrasId,
                        principalTable: "Zanrai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NuotaikosZanrai_ZanrasId",
                table: "NuotaikosZanrai",
                column: "ZanrasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NuotaikosZanrai");

            migrationBuilder.AddColumn<string>(
                name: "ZanrasIds",
                table: "Nuotaikos",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ZanrasIds",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "ZanrasIds",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ZanrasIds",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ZanrasIds",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "ZanrasIds",
                value: "[]");
        }
    }
}
