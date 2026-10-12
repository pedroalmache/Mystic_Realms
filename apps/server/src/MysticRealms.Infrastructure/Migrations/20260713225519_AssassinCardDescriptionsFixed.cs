using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssassinCardDescriptionsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 104,
                column: "Description",
                value: "Inflict {{Poison}} {{Poison_IMG}}{{NL}}on an enemy");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 105,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Exhaust");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 106,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}} to your{{NL}}hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 107,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}twice");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 108,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}{{NL}}Add {{EffectMultiplier}} {{Throwing_Knife}}{{NL}}to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 109,
                column: "Description",
                value: "Draw {{Draw}} {{Card_IMG}}{{NL}}Add {{EffectMultiplier}} {{Throwing_Knife}}{{NL}}to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 110,
                column: "Description",
                value: "{{Poison_IMG}} deals{{NL}}{{EffectMultiplier}} more damage{{NL}}this combat");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 111,
                column: "Description",
                value: "Gain {{Strength}} {{Strength_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 112,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}}{{NL}}to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 113,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Deal extra {{Damage_IMG}}{{NL}}equal to the target's {{Poison_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "Daggers", "Description" },
                values: new object[] { 9, "{{Dagger_IMG}} Deal {{Attack}} {{Damage_IMG}}" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 115,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Deals 2x if played{{NL}}as your first {{Card_IMG}} this turn");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 116,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}to ALL enemies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 117,
                column: "Description",
                value: "Your attacks{{NL}}apply {{Poison}} {{Poison_IMG}}{{NL}}this turn.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 118,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Apply {{Poison}} {{Poison_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 119,
                column: "Description",
                value: "Consume all {{Dagger}}{{NL}}Deal {{Attack}} {{Damage_IMG}}{{NL}}for each consumed.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 120,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}}{{NL}}to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 121,
                column: "Description",
                value: "Whenever you consume a{{NL}}{{Dagger}}, gain {{Strength}} {{Strength_IMG}}{{NL}}this turn");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 122,
                column: "Description",
                value: "Whenever you trigger Poison, draw {{Draw}} {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 123,
                column: "Description",
                value: "Whenever you trigger Poison, reset your {{Dagger_IMG}} to 3");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 124,
                column: "Description",
                value: "Whenever you play {{Throwing_Knife}}, add another {{Throwing_Knife}} to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 125,
                column: "Description",
                value: "Target enemy takes {{EffectMultiplier}}x Poison damage");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 126,
                column: "Description",
                value: "After playing 9 {{Throwing_Knife}}, gain {{Strength}} {{Strength_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 127,
                column: "Description",
                value: "At the start of your turn, add {{EffectMultiplier}} {{Throwing_Knife}} to your hand");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 104,
                column: "Description",
                value: "Inflict {{Poison}} {{Poison_IMG}} on an enemy");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 105,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}. Exhaust.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 106,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 107,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}} twice.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 108,
                column: "Description",
                value: "Gain {{Armor}} {{Armor_IMG}}. Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 109,
                column: "Description",
                value: "Draw {{Draw}} {{Card_IMG}}. Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 110,
                column: "Description",
                value: "Your Poison deals {{EffectMultiplier}} more damage this combat.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 111,
                column: "Description",
                value: "Gain {{Daggers}} {{Dagger_IMG}}. Gain {{Strength}} {{Strength_IMG}} this turn.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 112,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 113,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}. Deal extra damage equal to the target's Poison.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "Daggers", "Description" },
                values: new object[] { 0, "Can only be played with 0 {{Dagger_IMG}}. Deal {{Attack}} {{Damage_IMG}}." });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 115,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}. Deals double if played as your first Attack this turn.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 116,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}} to ALL enemies.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 117,
                column: "Description",
                value: "Your attacks apply {{Poison}} {{Poison_IMG}} this turn.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 118,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}. Apply {{Poison}} {{Poison_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 119,
                column: "Description",
                value: "Consume all {{Dagger_IMG}}. Deal {{Attack}} {{Damage_IMG}} for each consumed.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 120,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 121,
                column: "Description",
                value: "Whenever you consume a {{Dagger_IMG}}, gain {{Strength}} {{Strength_IMG}} this turn.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 122,
                column: "Description",
                value: "Whenever you trigger Poison, draw {{Draw}} {{Card_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 123,
                column: "Description",
                value: "Whenever you trigger Poison, reset your {{Dagger_IMG}} to 3.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 124,
                column: "Description",
                value: "Whenever you play {{Throwing_Knife}}, add another {{Throwing_Knife}} to your hand.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 125,
                column: "Description",
                value: "Target enemy takes {{EffectMultiplier}}x Poison damage.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 126,
                column: "Description",
                value: "After playing 9 {{Throwing_Knife}}, gain {{Strength}} {{Strength_IMG}}.");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 127,
                column: "Description",
                value: "At the start of your turn, add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.");
        }
    }
}
