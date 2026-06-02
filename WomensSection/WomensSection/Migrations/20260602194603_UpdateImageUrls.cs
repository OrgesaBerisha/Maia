using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
