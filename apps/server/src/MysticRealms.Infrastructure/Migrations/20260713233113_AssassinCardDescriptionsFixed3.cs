using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssassinCardDescriptionsFixed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 115,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}Deals 2x if played{{NL}}as your first {{Card_IMG}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 116,
                column: "Description",
                value: "Deal {{Attack}} {{Damage_IMG}}{{NL}}to All enemies");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "Whenever you apply {{Poison}}, draw {{Draw}} {{Card_IMG}}", true });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "Whenever you apply {{Poison_IMG}}, reset your {{Dagger_IMG}} to 2", true });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 124,
                column: "Description",
                value: "Whenever you gain {{Throwing_Knife}}, gain another {{Throwing_Knife}}");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 127,
                column: "Description",
                value: "At the start of your turn, gain {{EffectMultiplier}} {{Throwing_Knife}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 122,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "Whenever you trigger Poison, draw {{Draw}} {{Card_IMG}}", false });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "Description", "Permanent" },
                values: new object[] { "Whenever you trigger Poison, reset your {{Dagger_IMG}} to 3", false });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 124,
                column: "Description",
                value: "Whenever you play {{Throwing_Knife}}, add another {{Throwing_Knife}} to your hand");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 127,
                column: "Description",
                value: "At the start of your turn, add {{EffectMultiplier}} {{Throwing_Knife}} to your hand");
        }
    }
}
