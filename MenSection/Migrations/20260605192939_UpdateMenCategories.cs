using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MenSection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMenCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Tops");

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Bottoms");

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Suits & Formalwear");

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Outerwear");

            migrationBuilder.InsertData(
                table: "MenCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Swimwear" },
                    { 6, "Footwear" },
                    { 7, "Accessories" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "T-Shirts");

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Jackets");

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Jeans");

            migrationBuilder.UpdateData(
                table: "MenCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Shirts");
        }
    }
}
