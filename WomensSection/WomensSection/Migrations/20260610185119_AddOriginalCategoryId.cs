using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginalCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalCategoryId",
                table: "CardsWomen",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 1,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 2,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 3,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 4,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 5,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 6,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 7,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 8,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 9,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 10,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 11,
                column: "OriginalCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CardsWomen",
                keyColumn: "Id",
                keyValue: 12,
                column: "OriginalCategoryId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalCategoryId",
                table: "CardsWomen");
        }
    }
}
