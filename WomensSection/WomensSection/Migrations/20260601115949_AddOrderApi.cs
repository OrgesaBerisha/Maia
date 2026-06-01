using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 11, 59, 48, 259, DateTimeKind.Utc).AddTicks(5495));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 11, 59, 48, 259, DateTimeKind.Utc).AddTicks(6207));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 11, 59, 48, 259, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 11, 59, 48, 259, DateTimeKind.Utc).AddTicks(6209));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 31, 21, 3, 46, 272, DateTimeKind.Utc).AddTicks(7243));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 31, 21, 3, 46, 272, DateTimeKind.Utc).AddTicks(7837));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 31, 21, 3, 46, 272, DateTimeKind.Utc).AddTicks(7839));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 31, 21, 3, 46, 272, DateTimeKind.Utc).AddTicks(7840));
        }
    }
}
