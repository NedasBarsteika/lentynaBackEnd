using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lentynaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnwantedForumColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prikabinta",
                table: "Temos");

            migrationBuilder.DropColumn(
                name: "susitikimo_data",
                table: "Balsavimai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "prikabinta",
                table: "Temos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "susitikimo_data",
                table: "Balsavimai",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
