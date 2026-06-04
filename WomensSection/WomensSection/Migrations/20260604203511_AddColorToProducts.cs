using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "CardsWomen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 1,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 2,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 3,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 4,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 5,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 6,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 7,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 8,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 9,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 10,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 11,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 12,
                column: "Color",
                value: null);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "CardsWomen");

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
    }
}
