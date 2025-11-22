using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Autoriai",
                columns: new[] { "Id", "curiculum_vitae", "gimimo_metai", "knygu_skaicius", "laidybe", "mirties_data", "nuotrauka", "pavarde", "vardas" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), "Lietuvių rašytojas, poetas, publicistas. Vienas žymiausių lietuvių novelistų.", new DateTime(1879, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, new DateTime(1907, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Biliūnas", "Jonas" },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), "Lietuvių rašytojas, dramaturgas, profesorius. Parašė daug dramų, apsakymų ir romanų.", new DateTime(1882, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, new DateTime(1954, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Krėvė-Mickevičius", "Vincas" },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), "Lietuvių poetas, dramaturgas, literatūros kritikas ir teatro teoretikas.", new DateTime(1896, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, new DateTime(1947, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sruoga", "Balys" },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), "Šiuolaikinė lietuvių rašytoja, meno istorikė. Garsėja istorinių romanų serija „Silva Rerum", new DateTime(1974, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, null, null, "Sabaliauskaitė", "Kristina" },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), "Anglų rašytojas ir žurnalistas, garsėjantis distopiniais romanais ir socialine kritika.", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, new DateTime(1950, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Orwell", "George" }
                });

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

            migrationBuilder.InsertData(
                table: "Knygos",
                columns: new[] { "Id", "AutoriusId", "ISBN", "aprasymas", "bestseleris", "knygos_pavadinimas", "leidimo_metai", "psl_skaicius", "raisos", "virselio_nuotrauka" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("b0000000-0000-0000-0000-000000000001"), "9785417012345", "Viena garsiausių J. Biliūno novelių apie vargšę mergaitę ir jos sunkų gyvenimą.", false, "Liūdna pasaka", new DateTime(1907, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("b0000000-0000-0000-0000-000000000001"), "9785417012346", "Novelė apie kaltės jausmą ir žmogaus sąžinę.", false, "Kliudžiau", new DateTime(1906, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("b0000000-0000-0000-0000-000000000002"), "9785417012347", "Istorinė drama apie Lietuvos kunigaikštį Skirgailą ir jo vidines kovas.", false, "Skirgaila", new DateTime(1925, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 180, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("b0000000-0000-0000-0000-000000000002"), "9785417012348", "Padavimų rinkinys iš Dzūkijos krašto.", false, "Dainavos šalies senų žmonių padavimai", new DateTime(1912, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 256, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("b0000000-0000-0000-0000-000000000003"), "9785417012349", "Memuarai apie autoriaus patirtį Štuthofo koncentracijos stovykloje.", true, "Dievų miškas", new DateTime(1957, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 320, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("b0000000-0000-0000-0000-000000000004"), "9789955234562", "Pirmoji istorinių romanų serijos knyga apie Narvoišių giminę XVII amžiaus Lietuvoje.", true, "Silva Rerum", new DateTime(2008, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 456, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("b0000000-0000-0000-0000-000000000004"), "9789955234563", "Antroji serijos knyga, tęsianti Narvoišių giminės istoriją.", true, "Silva Rerum II", new DateTime(2011, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 512, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("b0000000-0000-0000-0000-000000000005"), "9780451524935", "Distopinis romanas apie totalitarinę visuomenę, kurioje valdžia kontroliuoja kiekvieną gyvenimo aspektą.", true, "1984", new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 328, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), new Guid("b0000000-0000-0000-0000-000000000005"), "9780451526342", "Alegorinė pasaka apie gyvulius, kurie sukyla prieš savo šeimininkus ir bando sukurti lygybės visuomenę.", true, "Gyvulių ūkis", new DateTime(1945, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 112, null, null }
                });

            migrationBuilder.InsertData(
                table: "KnygaNuotaikos",
                columns: new[] { "KnygaId", "NuotaikaId" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.InsertData(
                table: "KnygaZanrai",
                columns: new[] { "KnygaId", "ZanrasId" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "KnygaNuotaikos",
                keyColumns: new[] { "KnygaId", "NuotaikaId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000009"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000008"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd") });

            migrationBuilder.DeleteData(
                table: "KnygaZanrai",
                keyColumns: new[] { "KnygaId", "ZanrasId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000009"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

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

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Knygos",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Autoriai",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Autoriai",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Autoriai",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Autoriai",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Autoriai",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000005"));
        }
    }
}
