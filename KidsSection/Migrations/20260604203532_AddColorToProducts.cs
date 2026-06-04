using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "KidsCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 1,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 2,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 3,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 4,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 5,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 6,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 7,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 8,
                column: "Color",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "KidsCards");
        }
    }
}
