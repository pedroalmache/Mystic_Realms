using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WarriorCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 124,
                column: "Name",
                value: "Endless Reserves");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 4, null, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, null, 0, "Your first attack each turn{{NL}}grants 1 additional{{NL}}{{Momentum_IMG}}", 0, null, 1, 0, null, 0, 0, 0, 2, null, "Banner of Advance", true, 0, 0, 2, null, 0, null },
                    { 5, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, null, 0, "Your {{Armor_IMG}} is not removed{{NL}}at the start of your turn", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Unbroken Line", true, 0, 0, 3, null, 0, null },
                    { 6, null, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, null, 0, "Consume all {{Momentum_IMG}}.{{NL}}Deal {{EffectMultiplier}} {{Damage_IMG}} per Momentum{{NL}}to all enemies", 0, null, 4, 0, null, 0, 0, 0, 2, null, "Final Push", false, 0, 0, 3, null, 0, null },
                    { 17, null, 0, 0, 10, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 7, 1, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, 2, "Vanguard Slash", false, 0, 0, 1, null, 0, null },
                    { 18, null, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, null, 0, "Deal {{Attack}} {{Damage_IMG}}{{NL}}to all enemies", 0, null, 0, 0, null, 0, 0, 0, 2, 2, "Sweeping Arc", false, 0, 0, 1, null, 0, null },
                    { 19, null, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0, "Draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, 2, "Tactical Advance", false, 0, 0, 1, null, 0, null },
                    { 20, null, 8, 12, 12, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 8, 1, null, 0, "Deal {{Attack}} {{Damage_IMG}}{{NL}}and gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 2, 1, "Shield Charge", false, 0, 0, 2, null, 0, null },
                    { 21, null, 9, 13, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0, "Gain {{Armor}} {{Armor_IMG}}{{NL}}If Momentum is consumed,{{NL}}draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Brace for Impact", false, 0, 0, 1, null, 0, null },
                    { 22, null, 0, 0, 7, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 5, 1, null, 0, "Consume up to 4 {{Momentum_IMG}}.{{NL}}Deal {{Attack}} {{Damage_IMG}} plus{{NL}}{{EffectMultiplier}} per Momentum", 0, null, 3, 1, null, 0, 0, 0, 1, null, "Momentum Strike", false, 0, 0, 2, null, 0, null },
                    { 23, null, 10, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0, "Gain {{Armor}} {{Armor_IMG}}.{{NL}}Lost Armor damages{{NL}}the attacker this turn", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Iron Reversal", false, 0, 0, 2, null, 0, null },
                    { 24, null, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, null, 0, "The first time you gain{{NL}}{{Momentum_IMG}} each turn,{{NL}}draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 2, null, "Warpath", true, 0, 0, 2, null, 0, null },
                    { 25, null, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, null, 1, "At the start of each turn,{{NL}}gain {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Banner of Defense", true, 0, 0, 2, null, 0, null },
                    { 26, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 1, null, 0, "At the start of each turn,{{NL}}gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Banner of Offense", true, 0, 0, 2, null, 1, null },
                    { 27, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 1, null, 0, "The first time you share{{NL}}{{Armor_IMG}} each turn,{{NL}}gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 1, 2, null, "Banner of Unity", true, 0, 0, 3, null, 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 124,
                column: "Name",
                value: "Endless Arsenal");
        }
    }
}
