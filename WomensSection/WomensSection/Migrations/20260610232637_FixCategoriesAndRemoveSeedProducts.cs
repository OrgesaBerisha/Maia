using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoriesAndRemoveSeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename categories to match frontend filter labels
            migrationBuilder.UpdateData(table: "WomanCategories", keyColumn: "Id", keyValue: 1, column: "Name", value: "Tops");
            migrationBuilder.UpdateData(table: "WomanCategories", keyColumn: "Id", keyValue: 2, column: "Name", value: "Dresses");
            migrationBuilder.UpdateData(table: "WomanCategories", keyColumn: "Id", keyValue: 3, column: "Name", value: "Bottoms");
            migrationBuilder.UpdateData(table: "WomanCategories", keyColumn: "Id", keyValue: 4, column: "Name", value: "Outerwear");
            migrationBuilder.UpdateData(table: "WomanCategories", keyColumn: "Id", keyValue: 5, column: "Name", value: "Swimwear");
            migrationBuilder.UpdateData(table: "WomanCategories", keyColumn: "Id", keyValue: 6, column: "Name", value: "Matching Sets");

            // Add missing categories (safe insert — idempotent)
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM WomanCategories WHERE Id=7) INSERT INTO WomanCategories (Id,Name,CreatedAt) VALUES (7,'Footwear',GETUTCDATE())");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM WomanCategories WHERE Id=8) INSERT INTO WomanCategories (Id,Name,CreatedAt) VALUES (8,'Accessories',GETUTCDATE())");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM WomanCategories WHERE Id=9) INSERT INTO WomanCategories (Id,Name,CreatedAt) VALUES (9,'Sale',GETUTCDATE())");

            // Remove old seed products (safe delete — idempotent)
            migrationBuilder.Sql("DELETE FROM CardsWomen WHERE Id IN (1,2,3,4,5,6,7,8,9,10,11,12)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CardsWomen",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "DiscountPercent", "ImageUrl", "OriginalCategoryId", "Price", "Title", "UpdatedAt", "WomanCategoryId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Light summer dress", null, "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80", null, 25m, "Summer Dress", null, 1 },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic white sneakers", null, "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80", null, 80m, "White Sneakers", null, 2 },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warm winter jacket", null, "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=400&q=80", null, 120m, "Winter Jacket", null, 3 },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium leather bag", null, "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=400&q=80", null, 200m, "Leather Bag", null, 4 },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful floral midi dress", null, "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", null, 35m, "Floral Midi Dress", null, 1 },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant blue summer dress", null, "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80", null, 45m, "Blue Summer Dress", null, 1 },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stunning red evening dress", null, "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80", null, 89m, "Red Evening Dress", null, 1 },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chic white mini dress", null, "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80", null, 55m, "White Mini Dress", null, 1 },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant heel sandals", null, "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80", null, 95m, "Heel Sandals", null, 2 },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleek black ankle boots", null, "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", null, 120m, "Black Ankle Boots", null, 2 },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic beige trench coat", null, "https://images.unsplash.com/photo-1539109136-461e6d5a08f3?w=400&q=80", null, 160m, "Trench Coat", null, 3 },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stylish crossbody bag", null, "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80", null, 75m, "Crossbody Bag", null, 4 }
                });
        }
    }
}
