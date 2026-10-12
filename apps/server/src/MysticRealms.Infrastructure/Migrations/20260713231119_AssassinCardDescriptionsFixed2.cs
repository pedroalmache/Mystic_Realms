using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssassinCardDescriptionsFixed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 106,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}}{{NL}}to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 113,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Deal extra {{Damage_IMG}}{{NL}}equal to target's {{Poison_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 114,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 106,
                column: "Description",
                value: "Add {{EffectMultiplier}} {{Throwing_Knife}} to your{{NL}}hand");

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
                column: "Description",
                value: "{{Dagger_IMG}} Deal {{Attack}} {{Damage_IMG}}");
        }
    }
}
