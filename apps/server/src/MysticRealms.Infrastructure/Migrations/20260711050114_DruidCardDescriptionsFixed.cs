using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DruidCardDescriptionsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 708,
                column: "Description",
                value: "Transform");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 709,
                column: "Description",
                value: "Transform");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 710,
                column: "Description",
                value: "Transform");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 711,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 712,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}and gain {{Armor}} {{Armor_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 713,
                column: "Description",
                value: "Gain 2 {{Speed_IMG}}{{NL}}and draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 714,
                column: "Description",
                value: "Draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 719,
                column: "Description",
                value: "{{Wolf_Spirit}}: Draw {{Draw}} {{Card_IMG}} when attacking");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 724,
                column: "Description",
                value: "Prevent all damage{{NL}}{{Tortoise_Spirit}}:Your attacks are reduced by half while in");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 725,
                column: "Description",
                value: "Heal {{Heal}} {{Heal_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 726,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}and reduce{{NL}}{{Speed_IMG}} by 3");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 727,
                column: "Description",
                value: "Transform");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 708,
                column: "Description",
                value: "Transform.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 709,
                column: "Description",
                value: "Transform.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 710,
                column: "Description",
                value: "Transform.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 711,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 712,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}and gain {{Armor}} {{Armor_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 713,
                column: "Description",
                value: "Gain 2 {{Speed_IMG}}{{NL}}and draw {{Draw}} {{Card_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 714,
                column: "Description",
                value: "Draw {{Draw}} {{Card_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 719,
                column: "Description",
                value: "{{Wolf_Spirit}}: Draw {{Draw}} {{Card_IMG}} when attacking.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 724,
                column: "Description",
                value: "Prevent all damage{{NL}}{{Tortoise_Spirit}}:Your attacks are reduced by half while in.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 725,
                column: "Description",
                value: "Heal {{Heal}} {{Heal_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 726,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}and reduce the target's {{Speed_IMG}} by 3.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 727,
                column: "Description",
                value: "Transform.");
        }
    }
}
