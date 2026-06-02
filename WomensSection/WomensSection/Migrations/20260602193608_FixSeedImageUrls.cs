using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "Classic white sneakers", "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80", "White Sneakers" });

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Warm winter jacket", "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=400&q=80" });

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "Premium leather bag", "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=400&q=80", "Leather Bag" });

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 36, 7, 115, DateTimeKind.Utc).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 36, 7, 115, DateTimeKind.Utc).AddTicks(3767));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 36, 7, 115, DateTimeKind.Utc).AddTicks(3768));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 36, 7, 115, DateTimeKind.Utc).AddTicks(3769));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "...");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "Sport shoes", "...", "Nike Shoes" });

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Warm jacket", "..." });

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "Fashion bag", "...", "Luxury Bag" });

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 18, 23, 1, 587, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 18, 23, 1, 587, DateTimeKind.Utc).AddTicks(5347));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 18, 23, 1, 587, DateTimeKind.Utc).AddTicks(5348));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 18, 23, 1, 587, DateTimeKind.Utc).AddTicks(5349));
        }
    }
}
