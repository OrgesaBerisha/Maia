using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountPercent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPercent",
                table: "KidsCards",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 1,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 2,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 3,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 4,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 5,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 6,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 7,
                column: "DiscountPercent",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 8,
                column: "DiscountPercent",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "KidsCards");
        }
    }
}
