using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class FixImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://picsum.photos/seed/dress/400/500");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://picsum.photos/seed/shoes/400/500");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://picsum.photos/seed/jacket/400/500");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://picsum.photos/seed/bag/400/500");

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 30, 12, 599, DateTimeKind.Utc).AddTicks(1989));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 30, 12, 599, DateTimeKind.Utc).AddTicks(2727));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 30, 12, 599, DateTimeKind.Utc).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 19, 30, 12, 599, DateTimeKind.Utc).AddTicks(2729));
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
                column: "ImageUrl",
                value: "...");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "...");

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "...");

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
