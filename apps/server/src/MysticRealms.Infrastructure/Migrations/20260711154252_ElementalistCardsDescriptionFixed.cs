using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ElementalistCardsDescriptionFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 608,
                column: "Description",
                value: "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Water}}+{{Earth}}: Gain{{NL}}{{Armor}} {{Armor_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 616,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Fire}}+{{Water}}: Deal{{NL}}{{MagicAttack}} {{Magic_Damage_IMG}} extra");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 617,
                column: "Description",
                value: "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Earth}}+{{Air}}: Deal 2x{{NL}}Damage");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 618,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Air}}: Deal{{NL}}{{MagicAttack}} {{Magic_Damage_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 619,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}: Repeat");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 620,
                column: "Description",
                value: "Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Gain {{Mana}} {{Mana_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 621,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Heal {{Heal}} {{Heal_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 622,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Hit all enemies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 623,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Repeat");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 624,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}: Deal {{MagicAttack}} {{Magic_Damage_IMG}} extra");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 625,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "Whenever you{{NL}}gain essence,{{NL}}gain {{Defense}} {{Defense_IMG}}", true });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 626,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "When you gain{{NL}}your 4th essence,{{NL}}gain {{Mana}} {{Mana_IMG}}", true });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 627,
                columns: new[] { "Description", "Heal" },
                values: new object[] { "Heal {{Heal}} {{Heal_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Gain {{Mana}} {{Mana_IMG}}", 14 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 608,
                column: "Description",
                value: "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Water}}+{{Earth}}: Gain {{Armor}} {{Armor_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 616,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Fire}}+{{Water}}: Deal {{MagicAttack}} {{Magic_Damage_IMG}} extra");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 617,
                column: "Description",
                value: "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Earth}}+{{Air}}: Deal 2x Damage");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 618,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}}+{{Air}}: Deal {{MagicAttack}} extra {{Magic_Damage_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 619,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}}+{{Water}}: Repeat this attack");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 620,
                column: "Description",
                value: "Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Gain {{Mana}} {{Mana_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 621,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Heal {{Heal}} {{Heal_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 622,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Hit all enemies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 623,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Repeat this attack");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 624,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Deal {{MagicAttack}} extra {{Magic_Damage_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 625,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "Whenever you gain essence,{{NL}}gain {{Defense}} {{Defense_IMG}}", false });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 626,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "When you gain your 4th essence,{{NL}}gain {{Mana}} {{Mana_IMG}}", false });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 627,
                columns: new[] { "Description", "Heal" },
                values: new object[] { "Heal {{Heal}} {{Heal_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Gain {{Mana}} {{Mana_IMG}}", 4 });
        }
    }
}
