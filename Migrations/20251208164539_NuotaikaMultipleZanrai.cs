using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class NuotaikaMultipleZanrai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ŽINGSNIS 1: Pridėti naują ZanrasIds stulpelį (JSON)
            migrationBuilder.AddColumn<string>(
                name: "ZanrasIds",
                table: "Nuotaikos",
                type: "json",
                nullable: false,
                defaultValue: "[]")
                .Annotation("MySql:CharSet", "utf8mb4");

            // ŽINGSNIS 2: Migruoti esamus duomenis (ZanrasId → JSON array)
            // Konvertuojame kiekvieno Nuotaika ZanrasId į JSON array su vienu elementu
            migrationBuilder.Sql(@"
                UPDATE Nuotaikos
                SET ZanrasIds = JSON_ARRAY(CAST(ZanrasId AS CHAR(36)))
                WHERE ZanrasId IS NOT NULL
            ");

            // ŽINGSNIS 3: Ištrinti foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Nuotaikos_Zanrai_ZanrasId",
                table: "Nuotaikos");

            // ŽINGSNIS 4: Ištrinti index
            migrationBuilder.DropIndex(
                name: "IX_Nuotaikos_ZanrasId",
                table: "Nuotaikos");

            // ŽINGSNIS 5: Ištrinti senąjį ZanrasId stulpelį
            migrationBuilder.DropColumn(
                name: "ZanrasId",
                table: "Nuotaikos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZanrasIds",
                table: "Nuotaikos");

            migrationBuilder.AddColumn<Guid>(
                name: "ZanrasId",
                table: "Nuotaikos",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ZanrasId",
                value: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "ZanrasId",
                value: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ZanrasId",
                value: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ZanrasId",
                value: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.UpdateData(
                table: "Nuotaikos",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "ZanrasId",
                value: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.CreateIndex(
                name: "IX_Nuotaikos_ZanrasId",
                table: "Nuotaikos",
                column: "ZanrasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nuotaikos_Zanrai_ZanrasId",
                table: "Nuotaikos",
                column: "ZanrasId",
                principalTable: "Zanrai",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
