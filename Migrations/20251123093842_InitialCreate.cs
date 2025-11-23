using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Autoriai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vardas = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pavarde = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gimimo_metai = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    mirties_data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    curiculum_vitae = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nuotrauka = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tautybe = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    knygu_skaicius = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autoriai", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Naudotojai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    slapyvardis = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    el_pastas = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    slaptazodis = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sukurimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    profilio_nuotrauka = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Naudotojai", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Zanrai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    pavadinimas = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zanrai", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Citatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    citatos_tekstas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    citatos_data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    citatos_saltinis = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AutoriusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citatos_Autoriai_AutoriusId",
                        column: x => x.AutoriusId,
                        principalTable: "Autoriai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Autoriaus_sekimai",
                columns: table => new
                {
                    NaudotojasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AutoriusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sekimo_pradzia = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autoriaus_sekimai", x => new { x.NaudotojasId, x.AutoriusId });
                    table.ForeignKey(
                        name: "FK_Autoriaus_sekimai_Autoriai_AutoriusId",
                        column: x => x.AutoriusId,
                        principalTable: "Autoriai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Autoriaus_sekimai_Naudotojai_NaudotojasId",
                        column: x => x.NaudotojasId,
                        principalTable: "Naudotojai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Knygos_rekomendacijos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    rekomendacijos_pradzia = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    rekomendacijos_pabaiga = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NaudotojasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knygos_rekomendacijos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Knygos_rekomendacijos_Naudotojai_NaudotojasId",
                        column: x => x.NaudotojasId,
                        principalTable: "Naudotojai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Temos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    pavadinimas = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tekstas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sukurimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    redagavimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    istrynimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    prikabinta = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NaudotojasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temos_Naudotojai_NaudotojasId",
                        column: x => x.NaudotojasId,
                        principalTable: "Naudotojai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Knygos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    knygos_pavadinimas = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    leidimo_metai = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    aprasymas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    psl_skaicius = table.Column<int>(type: "int", nullable: true),
                    ISBN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    virselio_nuotrauka = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    kalba = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bestseleris = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AutoriusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ZanrasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knygos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Knygos_Autoriai_AutoriusId",
                        column: x => x.AutoriusId,
                        principalTable: "Autoriai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Knygos_Zanrai_ZanrasId",
                        column: x => x.ZanrasId,
                        principalTable: "Zanrai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nuotaikos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    pavadinimas = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZanrasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nuotaikos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nuotaikos_Zanrai_ZanrasId",
                        column: x => x.ZanrasId,
                        principalTable: "Zanrai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Balsavimai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    balsavimo_pradzia = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    balsavimo_pabaiga = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isrinkta_knyga_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    uzbaigtas = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    susitikimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balsavimai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balsavimai_Knygos_isrinkta_knyga_id",
                        column: x => x.isrinkta_knyga_id,
                        principalTable: "Knygos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DI_Komentarai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sugeneravimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    tekstas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modelis = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KnygaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DI_Komentarai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DI_Komentarai_Knygos_KnygaId",
                        column: x => x.KnygaId,
                        principalTable: "Knygos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Irasai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    tipas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sukurimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    redagavimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NaudotojasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnygaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnygosRekomendacijaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Irasai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Irasai_Knygos_KnygaId",
                        column: x => x.KnygaId,
                        principalTable: "Knygos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Irasai_Knygos_rekomendacijos_KnygosRekomendacijaId",
                        column: x => x.KnygosRekomendacijaId,
                        principalTable: "Knygos_rekomendacijos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Irasai_Naudotojai_NaudotojasId",
                        column: x => x.NaudotojasId,
                        principalTable: "Naudotojai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Komentarai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    komentaro_tekstas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    komentaro_data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    vertinimas = table.Column<int>(type: "int", nullable: false),
                    redagavimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NaudotojasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnygaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TemaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentarai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentarai_Knygos_KnygaId",
                        column: x => x.KnygaId,
                        principalTable: "Knygos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentarai_Naudotojai_NaudotojasId",
                        column: x => x.NaudotojasId,
                        principalTable: "Naudotojai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentarai_Temos_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temos",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Balsai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    pateikimo_data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NaudotojasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BalsavimasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnygaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balsai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balsai_Balsavimai_BalsavimasId",
                        column: x => x.BalsavimasId,
                        principalTable: "Balsavimai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Balsai_Knygos_KnygaId",
                        column: x => x.KnygaId,
                        principalTable: "Knygos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Balsai_Naudotojai_NaudotojasId",
                        column: x => x.NaudotojasId,
                        principalTable: "Naudotojai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Autoriai",
                columns: new[] { "Id", "curiculum_vitae", "gimimo_metai", "knygu_skaicius", "mirties_data", "nuotrauka", "pavarde", "tautybe", "vardas" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), "Lietuviu rasytojas, poetas, publicistas.", new DateTime(1879, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1907, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Biliunas", "Lietuvis", "Jonas" },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), "Lietuviu rasytojas, dramaturgas.", new DateTime(1882, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1954, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Kreve-Mickevicius", "Lietuvis", "Vincas" },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), "Lietuviu poetas, dramaturgas.", new DateTime(1896, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1947, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sruoga", "Lietuvis", "Balys" },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), "Anglu rasytojas ir zurnalistas.", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1950, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Orwell", "Anglas", "George" }
                });

            migrationBuilder.InsertData(
                table: "Zanrai",
                columns: new[] { "Id", "pavadinimas" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Trileriai" },
                    { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Poezija" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Fantastika" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Detektyvai" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Romanai" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Mokslune fantastika" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Istoriniai" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Biografijos" }
                });

            migrationBuilder.InsertData(
                table: "Knygos",
                columns: new[] { "Id", "AutoriusId", "ISBN", "ZanrasId", "aprasymas", "bestseleris", "kalba", "knygos_pavadinimas", "leidimo_metai", "psl_skaicius", "virselio_nuotrauka" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("b0000000-0000-0000-0000-000000000001"), "9785417012345", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Viena garsiausiu J. Biliuno noveliu.", false, "Lietuviu", "Liudna pasaka", new DateTime(1907, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, null },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("b0000000-0000-0000-0000-000000000001"), "9785417012346", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Novele apie kaltes jausma.", false, "Lietuviu", "Kliudziau", new DateTime(1906, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, null },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("b0000000-0000-0000-0000-000000000002"), "9785417012347", new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Istorine drama apie Lietuvos kunigaiksti.", false, "Lietuviu", "Skirgaila", new DateTime(1925, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 180, null },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("b0000000-0000-0000-0000-000000000003"), "9785417012349", new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Memuarai apie Stuthofo koncentracijos stovykla.", true, "Lietuviu", "Dievu miskas", new DateTime(1957, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 320, null },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("b0000000-0000-0000-0000-000000000004"), "9780451524935", new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Distopinis romanas apie totalitarine visuomene.", true, "Anglu", "1984", new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 328, null },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("b0000000-0000-0000-0000-000000000004"), "9780451526342", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Alegorine pasaka apie gyvulius.", true, "Anglu", "Gyvuliu ukis", new DateTime(1945, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 112, null }
                });

            migrationBuilder.InsertData(
                table: "Nuotaikos",
                columns: new[] { "Id", "ZanrasId", "pavadinimas" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Dziugi" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Liudna" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Neutrali" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Itemptas" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Romantiskas" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autoriaus_sekimai_AutoriusId",
                table: "Autoriaus_sekimai",
                column: "AutoriusId");

            migrationBuilder.CreateIndex(
                name: "IX_Balsai_BalsavimasId",
                table: "Balsai",
                column: "BalsavimasId");

            migrationBuilder.CreateIndex(
                name: "IX_Balsai_KnygaId",
                table: "Balsai",
                column: "KnygaId");

            migrationBuilder.CreateIndex(
                name: "IX_Balsai_NaudotojasId",
                table: "Balsai",
                column: "NaudotojasId");

            migrationBuilder.CreateIndex(
                name: "IX_Balsavimai_isrinkta_knyga_id",
                table: "Balsavimai",
                column: "isrinkta_knyga_id");

            migrationBuilder.CreateIndex(
                name: "IX_Citatos_AutoriusId",
                table: "Citatos",
                column: "AutoriusId");

            migrationBuilder.CreateIndex(
                name: "IX_DI_Komentarai_KnygaId",
                table: "DI_Komentarai",
                column: "KnygaId");

            migrationBuilder.CreateIndex(
                name: "IX_Irasai_KnygaId",
                table: "Irasai",
                column: "KnygaId");

            migrationBuilder.CreateIndex(
                name: "IX_Irasai_KnygosRekomendacijaId",
                table: "Irasai",
                column: "KnygosRekomendacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Irasai_NaudotojasId",
                table: "Irasai",
                column: "NaudotojasId");

            migrationBuilder.CreateIndex(
                name: "IX_Knygos_AutoriusId",
                table: "Knygos",
                column: "AutoriusId");

            migrationBuilder.CreateIndex(
                name: "IX_Knygos_ISBN",
                table: "Knygos",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Knygos_ZanrasId",
                table: "Knygos",
                column: "ZanrasId");

            migrationBuilder.CreateIndex(
                name: "IX_Knygos_rekomendacijos_NaudotojasId",
                table: "Knygos_rekomendacijos",
                column: "NaudotojasId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentarai_KnygaId",
                table: "Komentarai",
                column: "KnygaId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentarai_NaudotojasId",
                table: "Komentarai",
                column: "NaudotojasId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentarai_TemaId",
                table: "Komentarai",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Naudotojai_el_pastas",
                table: "Naudotojai",
                column: "el_pastas",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Naudotojai_slapyvardis",
                table: "Naudotojai",
                column: "slapyvardis",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nuotaikos_ZanrasId",
                table: "Nuotaikos",
                column: "ZanrasId");

            migrationBuilder.CreateIndex(
                name: "IX_Temos_NaudotojasId",
                table: "Temos",
                column: "NaudotojasId");

            migrationBuilder.CreateIndex(
                name: "IX_Zanrai_pavadinimas",
                table: "Zanrai",
                column: "pavadinimas",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Autoriaus_sekimai");

            migrationBuilder.DropTable(
                name: "Balsai");

            migrationBuilder.DropTable(
                name: "Citatos");

            migrationBuilder.DropTable(
                name: "DI_Komentarai");

            migrationBuilder.DropTable(
                name: "Irasai");

            migrationBuilder.DropTable(
                name: "Komentarai");

            migrationBuilder.DropTable(
                name: "Nuotaikos");

            migrationBuilder.DropTable(
                name: "Balsavimai");

            migrationBuilder.DropTable(
                name: "Knygos_rekomendacijos");

            migrationBuilder.DropTable(
                name: "Temos");

            migrationBuilder.DropTable(
                name: "Knygos");

            migrationBuilder.DropTable(
                name: "Naudotojai");

            migrationBuilder.DropTable(
                name: "Autoriai");

            migrationBuilder.DropTable(
                name: "Zanrai");
        }
    }
}
