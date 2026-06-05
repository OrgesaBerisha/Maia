using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class AddJewelrySwimwearCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WomanCategories",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, new DateTime(2026, 6, 5, 17, 55, 37, 0, DateTimeKind.Utc), "Jewelry", null },
                    { 6, new DateTime(2026, 6, 5, 17, 55, 37, 0, DateTimeKind.Utc), "Swimwear", null }
                });

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 5, 17, 55, 36, 238, DateTimeKind.Utc).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 5, 17, 55, 36, 238, DateTimeKind.Utc).AddTicks(5360));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 5, 17, 55, 36, 238, DateTimeKind.Utc).AddTicks(5361));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 5, 17, 55, 36, 238, DateTimeKind.Utc).AddTicks(5362));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(6826));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(7299));

            migrationBuilder.UpdateData(
                table: "WomanCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(7299));
        }
    }
}
