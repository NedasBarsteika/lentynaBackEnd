using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class SeedNuotaikosZanraiData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NuotaikosZanrai",
                columns: new[] { "NuotaikaId", "ZanrasId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.DeleteData(
                table: "NuotaikosZanrai",
                keyColumns: new[] { "NuotaikaId", "ZanrasId" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });
        }
    }
}
