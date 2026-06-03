using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CardsWomen",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Price", "Title", "UpdatedAt", "WomanCategoryId" },
                values: new object[,]
                {
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful floral midi dress", "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", 35m, "Floral Midi Dress", null, 1 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant blue summer dress", "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80", 45m, "Blue Summer Dress", null, 1 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stunning red evening dress", "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80", 89m, "Red Evening Dress", null, 1 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chic white mini dress", "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80", 55m, "White Mini Dress", null, 1 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant heel sandals", "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80", 95m, "Heel Sandals", null, 2 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleek black ankle boots", "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", 120m, "Black Ankle Boots", null, 2 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic beige trench coat", "https://images.unsplash.com/photo-1539109136-461e6d5a08f3?w=400&q=80", 160m, "Trench Coat", null, 3 },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stylish crossbody bag", "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80", 75m, "Crossbody Bag", null, 4 }
                });

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 20, 46, 16, 328, DateTimeKind.Utc).AddTicks(5424));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 20, 46, 16, 328, DateTimeKind.Utc).AddTicks(6287));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 20, 46, 16, 328, DateTimeKind.Utc).AddTicks(6289));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 20, 46, 16, 328, DateTimeKind.Utc).AddTicks(6290));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 46, 2, 459, DateTimeKind.Utc).AddTicks(7553));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 46, 2, 459, DateTimeKind.Utc).AddTicks(8062));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 46, 2, 459, DateTimeKind.Utc).AddTicks(8064));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 46, 2, 459, DateTimeKind.Utc).AddTicks(8065));
        }
    }
}
