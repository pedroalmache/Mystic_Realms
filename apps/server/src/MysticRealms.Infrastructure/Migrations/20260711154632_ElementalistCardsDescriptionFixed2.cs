using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ElementalistCardsDescriptionFixed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 618,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Air}}: Deal{{MagicAttack}} {{Magic_Damage_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 624,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}:{{NL}}Deal {{MagicAttack}} {{Magic_Damage_IMG}} extra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 618,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Air}}: Deal{{NL}}{{MagicAttack}} {{Magic_Damage_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 624,
                column: "Description",
                value: "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}to all enemies{{NL}}{{Fire}}+{{Water}}+{{Earth}}+{{Air}}: Deal {{MagicAttack}} {{Magic_Damage_IMG}} extra");
        }
    }
}
