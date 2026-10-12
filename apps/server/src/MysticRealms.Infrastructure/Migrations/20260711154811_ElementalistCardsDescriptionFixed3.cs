using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ElementalistCardsDescriptionFixed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
