using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtraAssassinCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 621,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Heal {{Heal}} {{Heal_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 623,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Repeat");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 126, null, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 1, 1, 0, 2, 0, 2, 0, 0, "After playing 9 {{Throwing_Knife}}, gain {{Strength}} {{Strength_IMG}}.", 0, null, 9, 0, null, 0, 0, 0, 2, null, "Nine Tails", true, 1, 0, 4, null, 1, null },
                    { 127, null, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, 1, 0, 0, 0, 2, 0, 0, "At the start of your turn, add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.", 0, null, 1, 0, null, 0, 0, 0, 2, null, "Arsenal", true, 1, 0, 4, null, 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 621,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}: Heal {{Heal}} {{Heal_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 623,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}: Repeat");
        }
    }
}
