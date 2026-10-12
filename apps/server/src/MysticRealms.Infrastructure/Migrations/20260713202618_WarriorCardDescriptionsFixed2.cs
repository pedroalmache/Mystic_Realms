using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WarriorCardDescriptionsFixed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "The first time{{NL}}you gain {{Momentum}}{{NL}}each turn,{{NL}}draw {{Draw}} {{Card_IMG}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "The first time{{NL}}you gain{{NL}}{{Momentum}}{{NL}}each turn,{{NL}}draw {{Draw}} {{Card_IMG}}");
        }
    }
}
