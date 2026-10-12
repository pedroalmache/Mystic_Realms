using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WarriorCardDescriptionsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Your first attack{{NL}}each turn grants{{NL}}1 additional {{Momentum}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Your {{Armor_IMG}} is not{{NL}}removed at the{{NL}}start of your{{NL}}turn");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Consume all {{Momentum}}{{NL}}Deal {{EffectMultiplier}} {{Damage_IMG}} per {{Momentum}}{{NL}}to all enemies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Deal {{Damage_IMG}} equal to{{NL}}your {{Armor_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Gain {{Strength}} {{Strength_IMG}} and{{NL}}{{Defense}} {{Defense_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}} and{{NL}}draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "Gain {{Strength}} {{Strength_IMG}} and{{NL}}draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 14,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Increase {{Damage_IMG}} by {{EffectMultiplier}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "All Allies gain{{NL}}{{Defense}} {{Defense_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 16,
                column: "Description",
                value: "Give {{Armor}} {{Armor_IMG}}{{NL}}to 2 allies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}If {{Momentum}} is consumed,{{NL}}draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "Consume up to 4 {{Momentum}}{{NL}}Deal {{Attack}} {{Damage_IMG}} plus{{NL}}{{EffectMultiplier}}x{{Momentum}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}Lost Armor damages{{NL}}the attacker");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "The first time{{NL}}you gain{{NL}}{{Momentum}}{{NL}}each turn,{{NL}}draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 25,
                column: "Description",
                value: "At the start of{{NL}}each turn,{{NL}}gain {{Defense}} {{Defense_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 26,
                column: "Description",
                value: "At the start of{{NL}}each turn,{{NL}}gain {{Strength}} {{Strength_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 27,
                column: "Description",
                value: "The first time{{NL}}you share {{Armor_IMG}}{{NL}}each turn,{{NL}}gain {{Mana}} {{Mana_IMG}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Your first attack each turn{{NL}}grants 1 additional{{NL}}{{Momentum_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Your {{Armor_IMG}} is not removed{{NL}}at the start of your turn");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Consume all {{Momentum_IMG}}.{{NL}}Deal {{EffectMultiplier}} {{Damage_IMG}} per Momentum{{NL}}to all enemies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Deal {{Damage_IMG}} equal to your {{Armor_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Gain {{Strength}} {{Strength_IMG}} and {{Defense}} {{Defense_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}} and draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "Gain {{Strength}} {{Strength_IMG}} and draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 14,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}. Increase {{Damage_IMG}} by {{EffectMultiplier}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "All Allies gain {{Defense}} {{Defense_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 16,
                column: "Description",
                value: "Give {{Armor}} {{Armor_IMG}} to 2 allies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}If Momentum is consumed,{{NL}}draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "Consume up to 4 {{Momentum_IMG}}.{{NL}}Deal {{Attack}} {{Damage_IMG}} plus{{NL}}{{EffectMultiplier}} per Momentum");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}.{{NL}}Lost Armor damages{{NL}}the attacker this turn");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "The first time you gain{{NL}}{{Momentum_IMG}} each turn,{{NL}}draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 25,
                column: "Description",
                value: "At the start of each turn,{{NL}}gain {{Defense}} {{Defense_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 26,
                column: "Description",
                value: "At the start of each turn,{{NL}}gain {{Strength}} {{Strength_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 27,
                column: "Description",
                value: "The first time you share{{NL}}{{Armor_IMG}} each turn,{{NL}}gain {{Mana}} {{Mana_IMG}}");
        }
    }
}
