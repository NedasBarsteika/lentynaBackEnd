using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserSeedFromMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Naudotojai",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Naudotojai",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Naudotojai",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Naudotojai",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000004"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Naudotojai",
                columns: new[] { "Id", "el_pastas", "profilio_nuotrauka", "role", "slaptazodis", "slapyvardis", "sukurimo_data" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "admin@lentyna.lt", null, "admin", "$2a$11$K7Qz5Y8X9Z0A1B2C3D4E5eFgHiJkLmNoPqRsTuVwXyZ0123456789a", "admin", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "moderatorius@lentyna.lt", null, "moderatorius", "$2a$11$K7Qz5Y8X9Z0A1B2C3D4E5eFgHiJkLmNoPqRsTuVwXyZ0123456789a", "moderatorius", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "redaktorius@lentyna.lt", null, "redaktorius", "$2a$11$K7Qz5Y8X9Z0A1B2C3D4E5eFgHiJkLmNoPqRsTuVwXyZ0123456789a", "redaktorius", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "skaitytojas@lentyna.lt", null, "naudotojas", "$2a$11$K7Qz5Y8X9Z0A1B2C3D4E5eFgHiJkLmNoPqRsTuVwXyZ0123456789a", "skaitytojas", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
