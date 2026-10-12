using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DruidCardDescriptionsFixed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 724,
                column: "Description",
                value: "Prevent all damage{{NL}}{{Tortoise_Spirit}}:Your attacks{{NL}}are reduced by half");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 724,
                column: "Description",
                value: "Prevent all damage{{NL}}{{Tortoise_Spirit}}:Your attacks are reduced by half while in");
        }
    }
}
